using ApexiBee.Application.Exceptions;
using ApexiBee.Application.Interfaces;
using ApexiBee.Application.Services;
using ApexiBee.Domain.Enums;
using ApexiBee.Domain.Models;
using ApexiBee.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Application.DomainServices
{
    public class EquipmentService : ServiceBase, IEquipmentService
    {
        public EquipmentService(IUnitOfWork unitOfWork) : base(unitOfWork)
        { }

        public async Task<SerialData> AddNewSerialData(EquipmentType equipmentType)
        {
            string serial = GenerateUniqueSerialNumber();
            SerialData newDevice = new SerialData() {
                EquipmentName = GetStringEquipmentName(equipmentType),
                SerialNumber = serial
            };

            await unitOfWork.SerialDataRepository.AddAsync(newDevice);
            await unitOfWork.SaveAsync();
            return newDevice;
        }

        public async Task<IEnumerable<SerialData>> GetAllSerialData()
        {
            return unitOfWork.SerialDataRepository.GetAll().ToList();
        }

        public async Task RemoveSerialDataById(Guid serialDataid)
        {
            SerialData? foundData = await unitOfWork.SerialDataRepository.GetByIdAsync(serialDataid);
            if(foundData == null)
            {
                throw new NotFoundException(serialDataid, "serial data");
            }

            unitOfWork.SerialDataRepository.Delete(foundData);
            await unitOfWork.SaveAsync();
        }

        public async Task RemoveSerialDataByName(string serialNumber)
        {
            SerialData? foundData = unitOfWork.SerialDataRepository.GetAll().FirstOrDefault(e => e.SerialNumber == serialNumber);
            if (foundData == null)
            {
                throw new NotFoundException("serial data");
            }

            unitOfWork.SerialDataRepository.Delete(foundData);
            await unitOfWork.SaveAsync();
        }

        public string GenerateUniqueSerialNumber()
        {
            string serial = "EQ";
            string timePart = $"{DateTime.UtcNow:yyyyMMdd}";
            string uniquePart = Guid.NewGuid().ToString("N").Substring(0, 8);
            string result = $"{serial}:{timePart}:{uniquePart}";

            return result;
        }

        public string GetStringEquipmentName(EquipmentType type)
        {
            switch(type)
            {
                case EquipmentType.Hive:
                    return "hive";
                case EquipmentType.Hub:
                    return "hub";
                case EquipmentType.Sensor:
                    return "sensor";
                default:
                    return "notdefined";
            }
        }
    }
}
