using ApexiBee.Application.DTO;
using ApexiBee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Application.Interfaces
{
    public interface IOrderService
    {
        Task<Order> PlaceOrder(NewOrderData orderData);

        Task<Order> ApproveOrder(Guid orderId);

        Task<Order> CompleteOrder(Guid orderId);

        Task<Order> CancelOrder(Guid orderId);

        Task<IEnumerable<Order>> GetUserOrders(Guid userId);

        Task<IEnumerable<Order>> GetAllOrders();

        Task<IEnumerable<Order>> GetOrdersWithFilters(OrderFilter filter);

        Task<OrderStatus> CreateNewStatus(string statusName);

        Task DeleteStatus(string statusName);

        Task<IEnumerable<OrderStatus>> GetOrderStatuses();
    }
}
