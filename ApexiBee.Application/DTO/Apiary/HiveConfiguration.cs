using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Application.DTO.Apiary
{
    public class HiveConfiguration
    {
        public Guid HiveId { get; set; }
        public Guid HumiditySensorId { get; set; }
        public Guid WeightSensorId { get; set; }
        public Guid TempSensorId { get; set; }
        public Guid HubId { get; set; }
        public double CriticalHumidityHigh { get; set; }
        public double CriticalHumidityLow { get; set; }
        public double CriticalTempHigh { get; set; }
        public double CriticalTempLow { get; set; }
    }
}
