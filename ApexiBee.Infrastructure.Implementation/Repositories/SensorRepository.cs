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
    public class SensorRepository : AbstractRepository, ISensorRepository
    {
        public SensorRepository(BeeDbContext context) : base(context)
        { }

        public async Task AddAsync(Sensor entity)
        {
            await context.Sensors.AddAsync(entity);
        }

        public void Delete(Sensor entity)
        {
            context.Sensors.Remove(entity);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var entity = await context.Sensors.FindAsync(id);
            if (entity == null)
            {
                throw new InvalidOperationException("Sensor not found");
            }

            context.Sensors.Remove(entity);
        }

        public IQueryable<Sensor> GetAll()
        {
            return context.Sensors.AsQueryable();
        }

        public async Task<Sensor?> GetByIdAsync(Guid id)
        {
            return await context.Sensors.FindAsync(id);
        }

        public async Task<Sensor?> GetByIdWithAllDetailsAsync(Guid id)
        {
            return await context.Sensors.Include(e => e.SensorReadings).Include(e => e.SensorType).FirstOrDefaultAsync(e => e.Id == id);
        }

        public void Update(Sensor entity)
        {
            context.Sensors.Update(entity);
        }
    }
}
