using ApexiBee.Infrastructure.Interfaces.Repository;
using ApexiBee.Infrastructure.Interfaces.Repository_Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        IApiaryRepository ApiaryRepository { get; }

        IHiveRepository HiveRepository { get; }

        IHubStationRepository HubStationRepository { get; }

        IOrderRepository OrderRepository { get; }

        IOrderStatusRepository OrderStatusRepository { get; }

        ISensorReadingRepository SensorReadingRepository { get; }

        ISensorTypeRepository SensorTypeRepository { get; }

        ISensorRepository SensorRepository { get; }

        ISerialDataRepository SerialDataRepository { get; }

        IUserRepository UserRepository { get; }

        Task SaveAsync();
    }
}
