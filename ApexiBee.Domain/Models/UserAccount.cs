using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Domain.Models
{
    public class UserAccount : BaseEntity
    {
        public DateTime RegistrationDate { get; set; }

        public IEnumerable<Apiary> Apiaries { get; set; }
    }
}
