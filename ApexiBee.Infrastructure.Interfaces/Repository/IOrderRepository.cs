using ApexiBee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Infrastructure.Interfaces.Repository
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order?> GetByIdWithAllDetailsAsync(Guid id);

        IQueryable<Order> GetAllWithAllDetails();
    }
}
