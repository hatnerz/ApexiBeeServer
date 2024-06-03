using ApexiBee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Infrastructure.Interfaces.Repository
{
    public interface IApiaryRepository : IRepository<Apiary>
    {
        IQueryable<Apiary> GetAllWithHives();

        Task<Apiary?> GetByIdWithHivesAsync(Guid id);
    }
}
