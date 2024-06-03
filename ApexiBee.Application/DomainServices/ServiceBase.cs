using ApexiBee.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Application.Services
{
    public abstract class ServiceBase
    {
        protected readonly IUnitOfWork unitOfWork;

        protected ServiceBase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
    }
}
