using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApexiBee.Domain.Models
{
    public class Sensor : BaseEntity
    {
        public Guid SensorTypeId { get; set; }
        public SensorType SensorType { get; set; }

        public Guid HiveId { get; set; }
        public Hive Hive { get; set; }

        public IEnumerable<SensorReading> SensorReadings { get; set; }
    }
}
