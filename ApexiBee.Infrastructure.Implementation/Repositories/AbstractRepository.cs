using ApexiBee.Persistance.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Infrastructure.Implementation.Repositories
{
    public class AbstractRepository
    {
        protected readonly BeeDbContext context;
        protected AbstractRepository(BeeDbContext context)
        {
            this.context = context;
        }
    }
}
