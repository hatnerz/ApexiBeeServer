using ApexiBee.Domain.Models;
using ApexiBee.Infrastructure.Interfaces.Repository;
using ApexiBee.Persistance.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Infrastructure.Implementation.Repositories
{
    public class SensorTypeRepository : AbstractRepository, ISensorTypeRepository
    {
        public SensorTypeRepository(BeeDbContext context) : base(context)
        { }

        public async Task AddAsync(SensorType entity)
        {
            await context.SensorTypes.AddAsync(entity);
        }

        public void Delete(SensorType entity)
        {
            context.SensorTypes.Remove(entity);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var entity = await context.SensorTypes.FindAsync(id);
            if (entity == null)
            {
                throw new InvalidOperationException("Sensor type not found");
            }

            context.SensorTypes.Remove(entity);
        }

        public IQueryable<SensorType> GetAll()
        {
             return context.SensorTypes.AsQueryable();
        }

        public async Task<SensorType?> GetByIdAsync(Guid id)
        {
            return await context.SensorTypes.FindAsync(id);
        }

        public void Update(SensorType entity)
        {
            context.SensorTypes.Update(entity);
        }
    }
}
