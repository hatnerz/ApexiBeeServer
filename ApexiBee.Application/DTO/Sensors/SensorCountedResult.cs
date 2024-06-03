using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Application.DTO
{
    public class SensorCountedResult
    {
        public Guid SensorId { get; set; }
        public Guid SensorTypeId { get; set; }
        public string SensorType { get; set; }
        public double? Value { get; set; }
        public string MeasureUnit { get; set; }
    }
}
