using ApexiBee.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Application.Interfaces
{
    public interface IAuthService
    {
        Task<bool> Register(RegisterUserData userData);

        Task<string?> Login(AuthUserData authData);

        Task<bool> AddRoleToUser(AddRoleToUserModel model);
    }
}
