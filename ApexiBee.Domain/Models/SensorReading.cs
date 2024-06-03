using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApexiBee.Domain.Models
{
    public class SensorReading : BaseEntity
    {
        public DateTime ReadingDate { get; set; }
        public double Value { get; set; }
        
        public Guid SensorId { get; set; }

        [JsonIgnore]
        public Sensor Sensor { get; set; }
    }
}
