using ApexiBee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Infrastructure.Interfaces.Repository
{
    public interface IHiveRepository : IRepository<Hive>
    {
        Task<Hive> GetByIdWithAllDetailsAsync(Guid id);
    }
}
