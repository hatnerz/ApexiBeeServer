using ApexiBee.Application.DTO;
using ApexiBee.Application.DTO.Apiary;
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
    public class ApiaryService : ServiceBase, IApiaryService
    {
        public ApiaryService(IUnitOfWork unitOfWork) : base(unitOfWork)
        { }

        public async Task<Hive> AddNewHive(NewHiveData hiveData)
        {
            Apiary? foundApiary = null;
            if(hiveData.ApiaryId.HasValue)
            {
                foundApiary = await unitOfWork.ApiaryRepository.GetByIdAsync((Guid)hiveData.ApiaryId);
                if(foundApiary == null)
                {
                    throw new NotFoundException("apiary");
                }
            }

            SerialData? serialData = unitOfWork.SerialDataRepository.GetAll()
                .FirstOrDefault(e => e.SerialNumber == hiveData.SerialNumber);

            if (serialData == null)
            {
                throw new NotFoundException("serial data");
            }

            if(serialData.EquipmentName.ToLower() != "hive")
            {
                throw new ArgumentException("Provided serial number corresponds to a different type of equipment", nameof(hiveData));
            }

            Hive? foundHive = unitOfWork.HiveRepository.GetAll().FirstOrDefault(e => e.SerialDataId == serialData.Id);
            if (foundHive != null)
            {
                throw new AlreadyExistsException("Hive", "serial number");
            }

            Hive newHive = new Hive()
            {
                Name = hiveData.Name,
                Description = hiveData.Description,
                IsActive = false,
                InstallationDate = DateTime.UtcNow,
                NumberOfFrames = hiveData.NumberOfFrames,
                Latitude = hiveData.Latitude,
                Longitude = hiveData.Longitude,
                ApiaryId = hiveData.ApiaryId,
                SerialDataId = serialData.Id
            };

            await unitOfWork.HiveRepository.AddAsync(newHive);
            await unitOfWork.SaveAsync();
            newHive.Apiary = null;
            return newHive;
        }

        public async Task CheckHive(Guid hiveId)
        {
            Hive? foundHive = await unitOfWork.HiveRepository.GetByIdAsync(hiveId);
            if(foundHive == null)
            {
                throw new NotFoundException(hiveId, "hive");
            }

            foundHive.LastInspectionDate = DateTime.UtcNow;
            foundHive.IsActive = true;
            await unitOfWork.SaveAsync();
        }

        public async Task<Apiary> CreateApiary(NewApiaryData apiaryData, NewHubStationData hubData)
        {
            UserAccount? foundBeekeeperAccount = await unitOfWork.UserRepository.GetByIdAsync(apiaryData.BeekeeperUserId);
            if(foundBeekeeperAccount == null)
            {
                throw new NotFoundException(apiaryData.BeekeeperUserId, "beekeeper");
            }

            Apiary apiary = new Apiary()
            {
                Id = Guid.NewGuid(),
                BeekeeperId = apiaryData.BeekeeperUserId,
                Name = apiaryData.Name,
                Description = apiaryData.Description,
                CreationDate = DateTime.UtcNow
            };

            hubData.ApiaryId = apiary.Id;

            HubStation hubStation = await CreateNewHubStationWithoutAdding(hubData);

            await unitOfWork.ApiaryRepository.AddAsync(apiary);
            await unitOfWork.HubStationRepository.AddAsync(hubStation);
            await unitOfWork.SaveAsync();
            return apiary;
        }

        public async Task<HubStation> CreateNewHubStationWithoutAdding(NewHubStationData hubData)
        {
            SerialData? foundSerial = unitOfWork.SerialDataRepository
                .GetAll().FirstOrDefault(e => e.SerialNumber.ToLower() == hubData.SerialNumber.ToLower());
            if (foundSerial == null)
            {
                throw new NotFoundException("serial number");
            }
            if (foundSerial.EquipmentName.ToLower() != "hub")
            {
                throw new ArgumentException("Provided serial number corresponds to a different type of equipment", nameof(hubData));
            }

            HubStation? foundHubStation = unitOfWork.HubStationRepository.GetAll().FirstOrDefault(e => e.SerialDataId == foundSerial.Id);
            if (foundHubStation != null)
            {
                throw new AlreadyExistsException("Hub station", "serial number");
            }

            HubStation hubStation = new HubStation()
            {
                Latitude = hubData.Latitude,
                Longitude = hubData.Longitude,
                EquipmentRegistrationDate = DateTime.UtcNow,
                SerialDataId = foundSerial.Id,
                ApiaryId = hubData.ApiaryId
            };

            return hubStation;
        }

        public async Task DeleteApiary(Guid apiaryId)
        {
            Apiary? foundApiary = await unitOfWork.ApiaryRepository.GetByIdWithHivesAsync(apiaryId);

            if(foundApiary == null)
            {
                throw new NotFoundException(apiaryId, "apiary");
            }

            foreach (var hive in foundApiary.Hives)
            {
                unitOfWork.HiveRepository.Delete(hive);
            }

            unitOfWork.HubStationRepository.Delete(foundApiary.Hub);
            unitOfWork.ApiaryRepository.Delete(foundApiary);
            await unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<Apiary>> GetAllApiariesWithHives()
        {
            return unitOfWork.ApiaryRepository.GetAllWithHives().ToList();
        }

        public async Task<IEnumerable<Hive>> GetApiaryHives(Guid apiaryId)
        {
            Apiary? foundApiary = await unitOfWork.ApiaryRepository.GetByIdWithHivesAsync(apiaryId);

            if (foundApiary == null)
            {
                throw new NotFoundException(apiaryId, "apiary");
            }

            return foundApiary.Hives;
        }

        public async Task<HiveConfiguration> GetHiveConfiguration(Guid hiveId)
        {
            Hive? foundHive = await unitOfWork.HiveRepository.GetByIdWithAllDetailsAsync(hiveId);
            if (foundHive == null)
                throw new NotFoundException(hiveId, "hive");

            Apiary? apiaryWithDetails = await unitOfWork.ApiaryRepository.GetByIdWithHivesAsync(foundHive.ApiaryId.Value);
            Sensor? humiditySensor = foundHive.Sensors.FirstOrDefault(e => e.SensorType.Name.ToLower() == "humidity");
            Sensor? weightSensor = foundHive.Sensors.FirstOrDefault(e => e.SensorType.Name.ToLower() == "weight");
            Sensor? tempSensor = foundHive.Sensors.FirstOrDefault(e => e.SensorType.Name.ToLower() == "temperature");

            if (apiaryWithDetails == null)
            {
                throw new NotFoundException("apiary");
            }

            if (humiditySensor == null || weightSensor == null || tempSensor == null)
            {
                throw new NotFoundException("one of the sensors");
            }

            HiveConfiguration hiveConfiguration = new HiveConfiguration()
            {
                HiveId = foundHive.Id,
                HumiditySensorId = humiditySensor.Id,
                WeightSensorId = weightSensor.Id,
                TempSensorId = tempSensor.Id,
                HubId = apiaryWithDetails.Hub.Id,
                CriticalTempHigh = apiaryWithDetails.Hub.CriticalTemperaruteHigh,
                CriticalTempLow = apiaryWithDetails.Hub.CriticalTemperatureLow,
                CriticalHumidityHigh = apiaryWithDetails.Hub.CriticalHumidityHigh,
                CriticalHumidityLow = apiaryWithDetails.Hub.CriticalHumidityLow
            };

            return hiveConfiguration;
        }

        public async Task<IEnumerable<Apiary>> GetUserApiaries(Guid userId)
        {
            UserAccount? foundUser = await unitOfWork.UserRepository.GetByIdAsync(userId);

            if(foundUser == null)
            {
                throw new NotFoundException(userId, "user account");
            }

            return unitOfWork.ApiaryRepository.GetAllWithHives().Where(e => e.BeekeeperId == userId).ToList();
        }

        public async Task RemoveHive(Guid hiveId)
        {
            Hive? foundHive = await unitOfWork.HiveRepository.GetByIdAsync(hiveId);

            if(foundHive == null)
            {
                throw new NotFoundException(hiveId, "hive");
            }

            unitOfWork.HiveRepository.Delete(foundHive);
            await unitOfWork.SaveAsync();
        }
    }
}
