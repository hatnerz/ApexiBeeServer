using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApexiBee.Domain.Models
{
    public class Hive : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime InstallationDate { get; set; }
        public DateTime LastInspectionDate { get; set; }
        public int NumberOfFrames { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Guid? ApiaryId { get; set; }

        [JsonIgnore]
        public Apiary Apiary { get; set; }

        public Guid? SerialDataId { get; set; }
        public SerialData SerialData { get; set; }

        public IEnumerable<Sensor> Sensors { get; set; } 
    }
}
