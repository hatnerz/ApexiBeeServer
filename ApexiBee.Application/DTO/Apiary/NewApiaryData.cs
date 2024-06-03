using ApexiBee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Application.DTO
{
    public class NewApiaryData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid BeekeeperUserId { get; set; }
    }
}
