using ApexiBee.Application.DTO;
using ApexiBee.Application.DTO.Sensors;
using ApexiBee.Application.Exceptions;
using ApexiBee.Application.Interfaces;
using ApexiBee.Application.Services;
using ApexiBee.Domain.Models;
using ApexiBee.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Application.DomainServices
{
    public class SensorService : ServiceBase, ISensorService
    {
        public SensorService(IUnitOfWork unitOfWork) : base(unitOfWork)
        { }

        public async Task<Sensor> AddNewSensor(NewSensorData newSensorData)
        {
            SensorType? foundSensorType = await unitOfWork.SensorTypeRepository.GetByIdAsync(newSensorData.SensorTypeId);
            if (foundSensorType == null)
            {
                throw new NotFoundException("sensor type");
            }

            Sensor newSensor = new Sensor()
            {
                SensorTypeId = newSensorData.SensorTypeId,
                HiveId = newSensorData.HiveId
            };

            await unitOfWork.SensorRepository.AddAsync(newSensor);
            await unitOfWork.SaveAsync();

            return newSensor;
        }

        public async Task<SensorType> AddNewSensorType(NewSensorTypeData newSensorTypeData)
        {
            SensorType sensorType = new SensorType()
            {
                Description = newSensorTypeData.Description,
                Name = newSensorTypeData.Name,
                MeasureUnit = newSensorTypeData.MeasureUnit
            };

            await unitOfWork.SensorTypeRepository.AddAsync(sensorType);
            await unitOfWork.SaveAsync();
            return sensorType;
        }

        public async Task<(int, int)> AddSensorReadings(IEnumerable<NewSensorReading> readings, Guid senderHubId)
        {
            int totalReadings = readings.Count();
            int addedReadings = 0;
            HubStation? foundHub = await unitOfWork.HubStationRepository.GetByIdAsync(senderHubId);
            if(foundHub == null)
            {
                throw new NotFoundException(senderHubId, "hub station");
            }

            Guid[] sensorIds = unitOfWork.SensorRepository.GetAll().Select(e => e.Id).ToArray();

            foreach(NewSensorReading reading in readings)
            {
                if (sensorIds.Contains(reading.SensorId))
                {
                    SensorReading newReading = new SensorReading()
                    {
                        SensorId = reading.SensorId,
                        Value = reading.Value,
                        ReadingDate = reading.ReadingDate
                    };
                    await unitOfWork.SensorReadingRepository.AddAsync(newReading);
                    addedReadings += 1;
                }
            }

            await unitOfWork.SaveAsync();
            return (addedReadings, totalReadings);
        }

        public async Task DeleteSensor(Guid sensorId)
        {
            try
            {
                await unitOfWork.SensorRepository.DeleteByIdAsync(sensorId);
            }
            catch (InvalidOperationException) 
            {
                throw new NotFoundException(sensorId, "sensor");
            }
        }

        public async Task<SensorCountedResult> GetAverageDailySensorValue(Guid sensorId, DateTime date)
        {
            Sensor? foundSensor = await unitOfWork.SensorRepository.GetByIdAsync(sensorId);
            if(foundSensor == null)
            {
                throw new NotFoundException(sensorId, "sensor");
            }

            SensorType sensorType = (await unitOfWork.SensorTypeRepository.GetByIdAsync(foundSensor.SensorTypeId))!;

            IEnumerable<SensorReading> sensorReadings = unitOfWork.SensorReadingRepository
                .GetAll().Where(e => e.ReadingDate.Date == date.Date && e.SensorId == sensorId).ToList();

            if(!sensorReadings.Any())
            {
                SensorCountedResult incorrectResult = new SensorCountedResult()
                {
                    SensorId = sensorId,
                    SensorTypeId = foundSensor.SensorTypeId,
                    Value = null,
                    SensorType = sensorType.Name,
                    MeasureUnit = sensorType.MeasureUnit
                };
                return incorrectResult;
            }

            double calculatedValue = CalculateWeightedAverage(sensorReadings.ToArray());

            SensorCountedResult result = new SensorCountedResult()
            {
                SensorId = sensorId,
                SensorTypeId = foundSensor.SensorTypeId,
                Value = calculatedValue,
                SensorType = sensorType.Name,
                MeasureUnit = sensorType.MeasureUnit
            };
            return result;

        }

        public async Task<IEnumerable<SensorReading>> GetLastHiveSensorData(Guid hiveId)
        {
            Hive? foundHive = await unitOfWork.HiveRepository.GetByIdWithAllDetailsAsync(hiveId);
            if(foundHive == null)
            {
                throw new NotFoundException(hiveId, "hive");
            }

            Guid[] sensorIds = foundHive.Sensors.Select(e => e.Id).ToArray();
            IEnumerable<SensorReading> hiveSensorReadings = unitOfWork.SensorReadingRepository
                .GetAll().Where(e => sensorIds.Contains(e.SensorId)).ToList();

            IEnumerable<SensorReading> lastSensorReadings = hiveSensorReadings
                .GroupBy(e => e.SensorId)
                .Select(g => g.OrderByDescending(e => e.ReadingDate).First())
                .ToList();

            return lastSensorReadings;
        }

        public async Task<IEnumerable<SensorReading>> GetSensorReadingsWithinPeriod(Guid sensorId, DateTime start, DateTime end)
        {
            Sensor? foundSensor = await unitOfWork.SensorRepository.GetByIdAsync(sensorId);
            if(foundSensor == null)
            {
                throw new NotFoundException(sensorId, "sensor");
            }

            IEnumerable<SensorReading> sensorReadingsWithinPeriod = unitOfWork.SensorReadingRepository
                .GetAll().Where(e => e.SensorId == sensorId && e.ReadingDate >= start && e.ReadingDate <= end).ToList();

            return sensorReadingsWithinPeriod;
        }

        public async Task<IEnumerable<SensorType>> GetSensorTypes()
        {
            var sensorTypes = unitOfWork.SensorTypeRepository.GetAll().ToList();
            return sensorTypes;
        }

        private double CalculateWeightedAverage(SensorReading[] readings)
        {
            if (readings == null || readings.Length == 0)
                throw new ArgumentException("The readings array is empty or null.");

            var sortedReadings = readings.OrderBy(r => r.ReadingDate).ToArray();

            double weightedSum = 0;
            double totalWeight = 0;

            for (int i = 0; i < sortedReadings.Length - 1; i++)
            {
                var currentReading = sortedReadings[i];
                var nextReading = sortedReadings[i + 1];

                double timeInterval = (nextReading.ReadingDate - currentReading.ReadingDate).TotalMinutes;

                double weight = timeInterval;

                weightedSum += currentReading.Value * weight;
                totalWeight += weight;
            }

            // Calculating last reading
            if (sortedReadings.Length > 1)
            {
                var lastReading = sortedReadings.Last();

                // We assume that the last reading is valid until the end of the day
                double lastWeight = (DateTime.Today.AddDays(1) - lastReading.ReadingDate).TotalMinutes;

                weightedSum += lastReading.Value * lastWeight;
                totalWeight += lastWeight;
            }

            return weightedSum / totalWeight;
        }
    }
}
