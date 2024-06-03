using ApexiBee.Application.DTO;
using ApexiBee.Application.Exceptions;
using ApexiBee.Application.Interfaces;
using ApexiBee.Application.Services;
using ApexiBee.Domain.Models;
using ApexiBee.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Application.DomainServices
{
    public class OrderService : ServiceBase, IOrderService
    {
        public OrderService(IUnitOfWork unitOfWork) : base(unitOfWork)
        { }

        public async Task<Order> ApproveOrder(Guid orderId)
        {
            Order? foundOrder = await unitOfWork.OrderRepository.GetByIdWithAllDetailsAsync(orderId);
            if (foundOrder == null || foundOrder.Status == null)
            {
                throw new NotFoundException(orderId, "order");
            }

            if (foundOrder.Status.StatusName.ToLower() == "canceled" || 
                foundOrder.Status.StatusName.ToLower() == "completed" ||
                foundOrder.Status.StatusName.ToLower() == "approved"
            )
            {
                throw new ArgumentException("Canceled, completed or approved orders cannot be approved", nameof(orderId));
            }

            OrderStatus? approvedStatus = unitOfWork.OrderStatusRepository.GetAll()
                .FirstOrDefault(e => e.StatusName.ToLower() == "approved");

            if (approvedStatus == null)
            {
                throw new NotFoundException("status");
            }

            foundOrder.Status = approvedStatus;
            foundOrder.OrderStatusId = approvedStatus.Id;
            await unitOfWork.SaveAsync();
            return foundOrder;
        }

        public async Task<Order> CancelOrder(Guid orderId)
        {
            Order? foundOrder = await unitOfWork.OrderRepository.GetByIdWithAllDetailsAsync(orderId);
            if (foundOrder == null || foundOrder.Status == null)
            {
                throw new NotFoundException(orderId, "order");
            }

            OrderStatus? canceledStatus = unitOfWork.OrderStatusRepository.GetAll()
                .FirstOrDefault(e => e.StatusName.ToLower() == "canceled");

            if (canceledStatus == null)
            {
                throw new NotFoundException("status");
            }

            foundOrder.Status = canceledStatus;
            foundOrder.OrderStatusId = canceledStatus.Id;
            await unitOfWork.SaveAsync();
            return foundOrder;
        }

        public async Task<Order> CompleteOrder(Guid orderId)
        {
            Order? foundOrder = await unitOfWork.OrderRepository.GetByIdWithAllDetailsAsync(orderId);
            if(foundOrder == null || foundOrder.Status == null)
            {
                throw new NotFoundException(orderId, "order");
            }

            if(foundOrder.Status.StatusName.ToLower() == "canceled" || foundOrder.Status.StatusName.ToLower() == "completed")
            {
                throw new ArgumentException("Canceled or completed orders cannot be completed", nameof(orderId));
            }

            OrderStatus? completedStatus = unitOfWork.OrderStatusRepository.GetAll()
                .FirstOrDefault(e => e.StatusName.ToLower() == "completed");
        
            if(completedStatus == null)
            {
                throw new NotFoundException("status");
            }

            foundOrder.Status = completedStatus;
            foundOrder.OrderStatusId = completedStatus.Id;
            await unitOfWork.SaveAsync();
            return foundOrder;
        }

        public async Task<OrderStatus> CreateNewStatus(string statusName)
        {
            statusName = statusName.ToLower();

            var foundStatus = unitOfWork.OrderStatusRepository
                .GetAll()
                .FirstOrDefault(e => e.StatusName == statusName);

            if(foundStatus != null)
            {
                throw new AlreadyExistsException("order status", "name");
            }

            OrderStatus orderStatus = new OrderStatus() { StatusName = statusName };
            await unitOfWork.OrderStatusRepository.AddAsync(orderStatus);
            await unitOfWork.SaveAsync();
            return orderStatus;
        }

        public async Task DeleteStatus(string statusName)
        {
            OrderStatus? status = unitOfWork.OrderStatusRepository.GetAll().FirstOrDefault(e => e.StatusName == statusName);
            if(status == null)
            {
                throw new NotFoundException("status");
            }

            unitOfWork.OrderStatusRepository.Delete(status);
            await unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return unitOfWork.OrderRepository.GetAllWithAllDetails().ToList();
        }

        public async Task<IEnumerable<Order>> GetOrdersWithFilters(OrderFilter filter)
        {
            var orderQuery = unitOfWork.OrderRepository.GetAllWithAllDetails();
            orderQuery = filter.MinDate != null ? orderQuery.Where(e => e.OrderDate >= filter.MinDate) : orderQuery;
            orderQuery = filter.MaxDate != null ? orderQuery.Where(e => e.OrderDate <= filter.MaxDate) : orderQuery;
            
            if(filter.Statuses != null && filter.Statuses.Any())
            {
                orderQuery = orderQuery.Where(e => filter.Statuses.Contains(e.Status!.StatusName));
            }

            return orderQuery.ToList();
        }

        public async Task<IEnumerable<OrderStatus>> GetOrderStatuses()
        {
            return unitOfWork.OrderStatusRepository.GetAll().ToList();
        }

        public async Task<IEnumerable<Order>> GetUserOrders(Guid userId)
        {
            Console.WriteLine(userId);
            return unitOfWork.OrderRepository.GetAllWithAllDetails().Where(e => e.BeekeeperId == userId).ToList();
        }

        public async Task<Order> PlaceOrder(NewOrderData orderData)
        {
            OrderStatus? orderStatus = unitOfWork.OrderStatusRepository.GetAll().FirstOrDefault(e => e.StatusName.ToLower() == "new");
            UserAccount? foundUser = await unitOfWork.UserRepository.GetByIdAsync(orderData.BeekeeperId);
            UserAccount? foundManager = await unitOfWork.UserRepository.GetByIdAsync(orderData.ManagerId);

            if (orderStatus == null)
            {
                throw new NotFoundException("status");
            }

            if(foundUser == null)
            {
                throw new NotFoundException(orderData.BeekeeperId, "beekeeper");
            }

            if(foundManager == null)
            {
                throw new NotFoundException(orderData.ManagerId, "manager");
            }

            Order order = new Order() 
            { 
                BeekeeperId = orderData.BeekeeperId,
                Description = orderData.Description,
                ManagerId = orderData.ManagerId,
                OrderStatusId = orderStatus.Id,
                OrderDate = DateTime.UtcNow
            };

            await unitOfWork.OrderRepository.AddAsync(order);
            await unitOfWork.SaveAsync();

            return order;
        }
    }
}
