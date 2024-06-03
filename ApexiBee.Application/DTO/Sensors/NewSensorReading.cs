using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Application.DTO.Sensors
{
    public class NewSensorReading
    {
        public double Value { get; set; }
        public Guid SensorId { get; set; }
        public DateTime ReadingDate { get; set; }
    }
}
