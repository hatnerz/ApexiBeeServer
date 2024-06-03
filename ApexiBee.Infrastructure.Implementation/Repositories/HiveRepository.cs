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
    public class HiveRepository : AbstractRepository, IHiveRepository
    {
        public HiveRepository(BeeDbContext context) : base(context)
        { }

        public async Task AddAsync(Hive entity)
        {
            await context.Hives.AddAsync(entity);
        }

        public void Delete(Hive entity)
        {
            context.Hives.Remove(entity);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var entity = await context.Hives.FindAsync(id);
            if (entity == null)
            {
                throw new InvalidOperationException("Hive not found");
            }

            context.Hives.Remove(entity);
        }

        public IQueryable<Hive> GetAll()
        {
            return context.Hives.AsQueryable();
        }

        public async Task<Hive?> GetByIdAsync(Guid id)
        {
            return await context.Hives.FindAsync(id);
        }

        public async Task<Hive?> GetByIdWithAllDetailsAsync(Guid id)
        {
            return await context.Hives.Include(e => e.Sensors).ThenInclude(e => e.SensorType).Include(e => e.Apiary).FirstOrDefaultAsync(e => e.Id == id);
        }

        public void Update(Hive entity)
        {
            throw new NotImplementedException();
        }
    }
}
