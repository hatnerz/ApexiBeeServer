using ApexiBee.Application.DTO;
using ApexiBee.Application.Exceptions;
using ApexiBee.Application.Helpers;
using ApexiBee.Application.Interfaces;
using ApexiBee.Domain.Models;
using ApexiBee.Infrastructure.Interfaces;
using ApexiBee.Persistance.Database;
using ApexiBee.Persistance.EntityConfiguration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Infrastructure.Implementation.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtHelper jwtHelper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole<Guid>> roleManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly BeeDbContext context;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            JwtHelper jwtHelper,
            IUnitOfWork unitOfWork,
            BeeDbContext context
        )
        {
            this.jwtHelper = jwtHelper;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.unitOfWork = unitOfWork;
            this.context = context;
        }

        public async Task<bool> Register(RegisterUserData userData)
        {
            var isUserWithEmailExists = await userManager.FindByEmailAsync(userData.Email);
            var isUserWithLoginExists = await userManager.FindByNameAsync(userData.Username);
            if (isUserWithEmailExists != null || isUserWithLoginExists != null)
            {
                throw new AlreadyExistsException("user", "email or login");
            }

            if (userData.Role == null || userData.Role == "")
            {
                userData.Role = "user";
            }

            var foundRole = await roleManager.FindByNameAsync(userData.Role.ToLower());
            if(foundRole == null)
            {
                throw new NotFoundException("role");
            }

            if(userData.Username == null)
            {
                userData.Username = userData.Email;
            }

            var userAccount = new UserAccount() { RegistrationDate = DateTime.UtcNow };
            await unitOfWork.UserRepository.AddAsync(userAccount);

            var newUser = new ApplicationUser() { Id = Guid.NewGuid(), UserName = userData.Username, Email = userData.Email, UserAccountId = userAccount.Id };
            var userCreatedResult = await userManager.CreateAsync(newUser, userData.Password);

            if(!userCreatedResult.Succeeded)
            {
                return false;
            }

            await AddRoleToUser(new AddRoleToUserModel() { role = userData.Role, username = userData.Username });
            return true;
        }

        public async Task<string?> Login(AuthUserData authData)
        {
            var foundUser = await userManager.FindByNameAsync(authData.Username);
            if(foundUser == null)
            {
                return null;
            }

            var passwordCheck = await userManager.CheckPasswordAsync(foundUser, authData.Password);
            if(!passwordCheck)
            {
                return null;
            }

            string roleName = "";

            var roles = await context.Roles
                .Where(r => context.UserRoles
                    .Where(ur => ur.UserId == foundUser.Id)
                    .Select(ur => ur.RoleId)
                    .Contains(r.Id))
                .ToListAsync();

            if(roles.Count > 0) 
            {
                roleName = String.Join(";", roles);
            }

            string jwt = jwtHelper.GenerateJwtToken(foundUser.UserAccountId, foundUser.Id, authData.Username, roleName);
            
            return jwt;
        }

        public async Task<bool> AddRoleToUser(AddRoleToUserModel model)
        {
            var foundUser = await this.userManager.FindByNameAsync(model.username);
            var foundRole = await this.roleManager.FindByNameAsync(model.role);

            if (foundUser == null || foundRole == null)
            {
                throw new NotFoundException("role or user");
            }

            var newUserRole = new IdentityUserRole<Guid>() { RoleId = foundRole.Id, UserId = foundUser.Id };
            await this.context.UserRoles.AddAsync(newUserRole);
            await this.context.SaveChangesAsync();

            return true;
        }
    }
}
