using ApexiBee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Application.DTO.Apiary
{
    public class HubCreatedResult
    {
        public Guid Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime EquipmentRegistrationDate { get; set; }
        public SerialData SerialData { get; set; }
    }
}
