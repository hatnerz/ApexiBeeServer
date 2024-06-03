using ApexiBee.Infrastructure.Interfaces;
using ApexiBee.Infrastructure.Interfaces.Repository;
using ApexiBee.Infrastructure.Interfaces.Repository_Interfaces;
using ApexiBee.Persistance.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Infrastructure.Implementation.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BeeDbContext _context;

        public UnitOfWork(BeeDbContext context)
        {

            this.ApiaryRepository = new ApiaryRepository(context);
            this.HiveRepository = new HiveRepository(context);
            this.HubStationRepository = new HubStationRepository(context);
            this.OrderRepository = new OrderRepository(context);
            this.OrderStatusRepository = new OrderStatusRepository(context);
            this.SensorReadingRepository = new SensorReadingRepository(context);
            this.SensorRepository = new SensorRepository(context);
            this.SensorTypeRepository = new SensorTypeRepository(context);
            this.SerialDataRepository = new SerialDataRepository(context);
            this.UserRepository = new UserRepository(context);
            _context = context;
        }

        public IApiaryRepository ApiaryRepository { get; private set; }

        public IHiveRepository HiveRepository { get; private set; }

        public IHubStationRepository HubStationRepository { get; private set; }

        public IOrderRepository OrderRepository { get; private set; }

        public IOrderStatusRepository OrderStatusRepository { get; private set; }

        public ISensorReadingRepository SensorReadingRepository { get; private set; }

        public ISensorRepository SensorRepository { get; private set; }

        public ISensorTypeRepository SensorTypeRepository { get; private set; }

        public ISerialDataRepository SerialDataRepository { get; private set; }

        public IUserRepository UserRepository { get; private set; }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
