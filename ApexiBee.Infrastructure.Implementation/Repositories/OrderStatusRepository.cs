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
    public class OrderStatusRepository : AbstractRepository, IOrderStatusRepository
    {
        public  OrderStatusRepository(BeeDbContext context) : base(context)
        { }

        public async Task AddAsync(OrderStatus entity)
        {
            await context.OrderStatuses.AddAsync(entity);
        }

        public void Delete(OrderStatus entity)
        {
            context.OrderStatuses.Remove(entity);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var entity = await context.OrderStatuses.FindAsync(id);
            if (entity == null)
            {
                throw new InvalidOperationException("Order status not found");
            }

            context.OrderStatuses.Remove(entity);
        }

        public IQueryable<OrderStatus> GetAll()
        {
            return context.OrderStatuses.AsQueryable();
        }

        public async Task<OrderStatus?> GetByIdAsync(Guid id)
        {
            return await context.OrderStatuses.FindAsync(id);
        }

        public void Update(OrderStatus entity)
        {
            context.OrderStatuses.Update(entity);
        }
    }
}
