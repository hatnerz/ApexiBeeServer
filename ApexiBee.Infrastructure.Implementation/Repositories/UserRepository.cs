using ApexiBee.Domain.Models;
using ApexiBee.Infrastructure.Interfaces.Repository_Interfaces;
using ApexiBee.Persistance.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Infrastructure.Implementation.Repositories
{
    public class UserRepository : AbstractRepository, IUserRepository
    {

        public UserRepository(BeeDbContext context) : base(context)
        { }

        public async Task AddAsync(UserAccount entity)
        {
            await context.UserAccounts.AddAsync(entity);
        }

        public void Delete(UserAccount entity)
        {
            context.UserAccounts.Remove(entity);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var entity = await context.UserAccounts.FindAsync(id);
            if (entity == null)
            {
                throw new InvalidOperationException("User not found");
            }

            context.UserAccounts.Remove(entity);
        }

        public IQueryable<UserAccount> GetAll()
        {
            return context.UserAccounts.AsQueryable();
        }

        public IQueryable<UserAccount> GetAllWithDetails()
        {
            return context.UserAccounts.Include(e => e.Apiaries);
        }

        public async Task<UserAccount?> GetByIdAsync(Guid id)
        {
            return await context.UserAccounts.FindAsync(id);
        }

        public async Task<UserAccount?> GetByIdWithDetailsAsync(Guid id)
        {
            return await context.UserAccounts.Include(e => e.Apiaries).FirstOrDefaultAsync(e => e.Id == id);
        }

        public void Update(UserAccount entity)
        {
            context.UserAccounts.Update(entity);
        }
    }
}
