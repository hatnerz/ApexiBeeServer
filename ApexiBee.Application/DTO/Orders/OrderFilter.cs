using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Application.DTO
{
    public class OrderFilter
    {
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public IEnumerable<string>? Statuses { get; set; }
    }
}
