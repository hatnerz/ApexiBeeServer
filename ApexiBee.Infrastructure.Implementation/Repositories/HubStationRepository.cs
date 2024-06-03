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
    public class HubStationRepository : AbstractRepository, IHubStationRepository
    {
        public HubStationRepository(BeeDbContext context) : base(context)
        { }

        public async Task AddAsync(HubStation entity)
        {
            await context.HubStations.AddAsync(entity);
        }

        public void Delete(HubStation entity)
        {
            context.HubStations.Remove(entity);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var entity = await context.HubStations.FindAsync(id);
            if (entity == null)
            {
                throw new InvalidOperationException("Hub station not found");
            }

            context.HubStations.Remove(entity);
        }

        public IQueryable<HubStation> GetAll()
        {
            return context.HubStations.AsQueryable();
        }

        public async Task<HubStation?> GetByIdAsync(Guid id)
        {
            return await context.HubStations.FindAsync(id);
        }

        public async Task<HubStation?> GetByIdWithAllDetailsAsync(Guid id)
        {
            return await context.HubStations.Include(e => e.SerialData).Include(e => e.Apiary).FirstOrDefaultAsync(e => e.Id == id);
        }

        public void Update(HubStation entity)
        {
            context.HubStations.Update(entity);
        }
    }
}
