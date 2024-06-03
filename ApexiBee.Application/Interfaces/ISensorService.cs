using ApexiBee.Application.DTO;
using ApexiBee.Application.DTO.Sensors;
using ApexiBee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Application.Interfaces
{
    public interface ISensorService
    {
        Task<IEnumerable<SensorType>> GetSensorTypes();

        Task<SensorType> AddNewSensorType(NewSensorTypeData newSensorTypeData);

        Task<Sensor> AddNewSensor(NewSensorData newSensorData);

        Task DeleteSensor(Guid sensorId);

        Task<(int, int)> AddSensorReadings(IEnumerable<NewSensorReading> readings, Guid senderHubId);

        Task<IEnumerable<SensorReading>> GetLastHiveSensorData(Guid hiveId);

        Task<IEnumerable<SensorReading>> GetSensorReadingsWithinPeriod(Guid sensorId, DateTime start, DateTime end);

        Task<SensorCountedResult> GetAverageDailySensorValue(Guid sensorId, DateTime date);
    }
}
