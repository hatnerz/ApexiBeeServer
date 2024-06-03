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
    public class OrderRepository : AbstractRepository, IOrderRepository
    {
        public OrderRepository(BeeDbContext context) : base(context)
        { }

        public async Task AddAsync(Order entity)
        {
            await context.Orders.AddAsync(entity);
        }

        public void Delete(Order entity)
        {
            context.Orders.Remove(entity);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var entity = await context.Orders.FindAsync(id);
            if (entity == null)
            {
                throw new InvalidOperationException("Order not found");
            }

            context.Orders.Remove(entity);
        }

        public IQueryable<Order> GetAll()
        {
            return context.Orders.AsQueryable();
        }

        public IQueryable<Order> GetAllWithAllDetails()
        {
            return context.Orders.Include(e => e.Status).Include(e => e.Beekeeper).Include(e => e.Manager);
        }

        public async Task<Order?> GetByIdAsync(Guid id)
        {
            return await context.Orders.FindAsync(id);
        }

        public async Task<Order?> GetByIdWithAllDetailsAsync(Guid id)
        {
            return await context.Orders.Include(e => e.Status).Include(e => e.Beekeeper).Include(e => e.Manager).FirstOrDefaultAsync(e => e.Id == id);
        }

        public void Update(Order entity)
        {
            context.Orders.Update(entity);
        }
    }
}
