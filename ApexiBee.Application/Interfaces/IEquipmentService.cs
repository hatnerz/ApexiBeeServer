using ApexiBee.Domain.Enums;
using ApexiBee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Application.Interfaces
{
    public interface IEquipmentService
    {
        /// <summary>
        /// Retrieves all serial data.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IEnumerable of SerialData.</returns>
        Task<IEnumerable<SerialData>> GetAllSerialData();
        
        /// <summary>
        /// Adds new serial data for the specified equipment type.
        /// </summary>
        /// <param name="equipmentType">The type of the equipment for which to add serial data.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the added SerialData.</returns>
        Task<SerialData> AddNewSerialData(EquipmentType equipmentType);

        /// <summary>
        /// Removes serial data by its serial number.
        /// </summary>
        /// <param name="serialNumber">The serial number of the serial data to remove.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task RemoveSerialDataByName(string serialNumber);

        /// <summary>
        /// Removes serial data by its ID.
        /// </summary>
        /// <param name="serialDataid">The ID of the serial data to remove.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task RemoveSerialDataById(Guid serialDataid);

        /// <summary>
        /// Generates a unique serial number.
        /// </summary>
        /// <returns>A unique serial number as a string.</returns>
        string GenerateUniqueSerialNumber();

        /// <summary>
        /// Gets the string representation of the equipment name based on the equipment type.
        /// </summary>
        /// <param name="type">The equipment type.</param>
        /// <returns>The string representation of the equipment name.</returns>
        string GetStringEquipmentName(EquipmentType type);
    }
}
