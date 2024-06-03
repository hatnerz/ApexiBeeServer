using ApexiBee.Domain.Models;
using ApexiBee.Infrastructure.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Infrastructure.Interfaces.Repository_Interfaces
{
    public interface IUserRepository : IRepository<UserAccount>
    {
        Task<UserAccount?> GetByIdWithDetailsAsync(Guid id);

        IQueryable<UserAccount> GetAllWithDetails();
    }
}
