using ApexiBee.Domain.Models;
using ApexiBee.Infrastructure.Interfaces.Repository;
using ApexiBee.Persistance.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Infrastructure.Implementation.Repositories
{
    public class ApiaryRepository : AbstractRepository, IApiaryRepository
    {
        public ApiaryRepository(BeeDbContext context) : base(context)
        { }

        public async Task AddAsync(Apiary entity)
        {
            await context.AddAsync(entity);
        }

        public void Delete(Apiary entity)
        {
            context.Remove(entity);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var foundApiary = await context.Apiaries.FindAsync(id);
            if (foundApiary == null) 
            {
                throw new InvalidOperationException("Apiary not found");
            }

            context.Apiaries.Remove(foundApiary);
        }

        public IQueryable<Apiary> GetAll()
        {
            return context.Apiaries.AsQueryable();
        }

        public IQueryable<Apiary> GetAllWithHives()
        {
            return context.Apiaries.Include(e => e.Hives).Include(e => e.Hub);
        }

        public async Task<Apiary?> GetByIdAsync(Guid id)
        {
            return await context.Apiaries.FindAsync(id);
        }

        public async Task<Apiary?> GetByIdWithHivesAsync(Guid id)
        {
            return await context.Apiaries.Include(e => e.Hives).Include(e => e.Hub).FirstOrDefaultAsync(e => e.Id == id);
        }

        public void Update(Apiary entity)
        {
            context.Apiaries.Update(entity);
        }
    }
}
