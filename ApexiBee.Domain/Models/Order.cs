using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Domain.Models
{
    public class Order : BaseEntity
    {
        public DateTime OrderDate { get; set; }
        public string Description { get; set; }

        public Guid OrderStatusId { get;set; }
        public OrderStatus Status { get;set; }
        
        public Guid? ManagerId { get; set; }
        public UserAccount? Manager { get; set; }
        
        public Guid? BeekeeperId { get; set; }
        public UserAccount? Beekeeper { get; set; }
    }
}
