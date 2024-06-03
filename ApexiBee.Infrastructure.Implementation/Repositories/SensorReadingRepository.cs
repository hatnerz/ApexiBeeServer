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
    public class SensorReadingRepository : AbstractRepository, ISensorReadingRepository
    {
        public SensorReadingRepository(BeeDbContext context) : base(context)
        { }

        public async Task AddAsync(SensorReading entity)
        {
            await context.SensorReadings.AddAsync(entity);
        }

        public void Delete(SensorReading entity)
        {
            context.SensorReadings.Remove(entity);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var entity = await context.SensorReadings.FindAsync(id);
            if (entity == null)
            {
                throw new InvalidOperationException("Sensor reading not found");
            }

            context.SensorReadings.Remove(entity);
        }

        public IQueryable<SensorReading> GetAll()
        {
            return context.SensorReadings.AsQueryable();
        }

        public async Task<SensorReading?> GetByIdAsync(Guid id)
        {
            return await context.SensorReadings.FindAsync(id);
        }

        public void Update(SensorReading entity)
        {
            context.SensorReadings.Update(entity);
        }
    }
}
