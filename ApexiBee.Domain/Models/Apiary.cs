using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApexiBee.Domain.Models
{
    public class Apiary : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }

        public Guid BeekeeperId { get; set; }

        public UserAccount Beekeeper { get; set; }

        public HubStation Hub { get; set; }

        public IEnumerable<Hive> Hives { get; set; }
    }
}
