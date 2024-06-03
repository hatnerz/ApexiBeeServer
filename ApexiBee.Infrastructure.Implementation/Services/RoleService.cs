using ApexiBee.Application.DTO;
using ApexiBee.Application.Exceptions;
using ApexiBee.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Infrastructure.Implementation.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole<Guid>> roleManager;

        public RoleService(RoleManager<IdentityRole<Guid>> roleManager) 
        {
            this.roleManager = roleManager;
        }

        public async Task<bool> AddRole(string roleName)
        {
            var foundRole = await roleManager.FindByNameAsync(roleName);
            if(foundRole != null)
            {
                throw new AlreadyExistsException("role", $"name({roleName})");
            }

            var newRole = new IdentityRole<Guid>(roleName.ToLower());
            var result = await roleManager.CreateAsync(newRole);

            if(!result.Succeeded)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> RemoveRole(string roleName)
        {
            var foundRole = await roleManager.FindByNameAsync(roleName);
            if(foundRole == null)
            {
                throw new NotFoundException("role");
            }

            var deletingResult = await roleManager.DeleteAsync(foundRole);
            if(!deletingResult.Succeeded)
            {
                return false;
            }

            return true;
        }
    }
}
