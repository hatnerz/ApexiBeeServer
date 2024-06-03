using ApexiBee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ApexiBee.Persistance.EntityConfiguration
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public Guid Id { get; set; }
        public Guid UserAccountId { get; set; }
        public UserAccount UserAccount { get; set; }
    }
}
