using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Domain.Models
{
    public class SerialData : BaseEntity
    {
        public string SerialNumber { get; set; }
        public string EquipmentName { get; set; }
    }
}
