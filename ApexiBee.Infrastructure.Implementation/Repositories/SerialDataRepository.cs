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
    public class SerialDataRepository : AbstractRepository, ISerialDataRepository
    {
        public SerialDataRepository(BeeDbContext context) : base(context)
        {
        }

        public async Task AddAsync(SerialData entity)
        {
            await context.SerialDatas.AddAsync(entity);
        }

        public void Delete(SerialData entity)
        {
            context.SerialDatas.Remove(entity);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var entity = await context.SerialDatas.FindAsync(id);
            if (entity == null)
            {
                throw new InvalidOperationException("Serial data not found");
            }

            context.SerialDatas.Remove(entity);
        }

        public IQueryable<SerialData> GetAll()
        {
            return context.SerialDatas.AsQueryable();
        }

        public async Task<SerialData?> GetByIdAsync(Guid id)
        {
            return await context.SerialDatas.FindAsync(id);
        }

        public void Update(SerialData entity)
        {
            context.SerialDatas.Update(entity);
        }
    }
}
