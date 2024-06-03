using ApexiBee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Infrastructure.Interfaces.Repository
{
    public interface ISensorRepository : IRepository<Sensor>
    {
        Task<Sensor?> GetByIdWithAllDetailsAsync(Guid id);
    }
}
