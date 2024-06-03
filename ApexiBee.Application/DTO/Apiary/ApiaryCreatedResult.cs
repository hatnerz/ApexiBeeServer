using ApexiBee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Application.DTO.Apiary
{
    public class ApiaryCreatedResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid BeekeeperId { get; set; }
        public HubCreatedResult Hub { get; set; }
    }
}
