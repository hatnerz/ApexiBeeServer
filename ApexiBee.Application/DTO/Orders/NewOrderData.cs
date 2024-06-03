using ApexiBee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Application.DTO
{
    public class NewOrderData
    {
        public string Description { get; set; }
        public Guid ManagerId { get; set; }
        public Guid BeekeeperId { get; set; }
    }
}
