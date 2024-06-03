using ApexiBee.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Application.Interfaces
{
    public interface IRoleService
    {
        Task<bool> AddRole(string roleName);

        Task<bool> RemoveRole(string roleName);
    }
}
