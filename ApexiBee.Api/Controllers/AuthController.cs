using ApexiBee.Application.DTO;
using ApexiBee.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApexiBee.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly IRoleService roleService;

        public AuthController(IAuthService authService, IRoleService roleService)
        {
            this.authService = authService;
            this.roleService = roleService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserData registerData)
        {
            var registerResult = await this.authService.Register(registerData);
            if(!registerResult)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthUserData authUserData)
        {
            var jwt = await this.authService.Login(authUserData);
            if (jwt == null)
            {
                return Unauthorized("Incorrect credentials");
            }

            return Ok(new { token = jwt });
        }

        [HttpPost("role")]
        public async Task<IActionResult> AddRole([FromBody] RoleData roleData)
        {
            var addResult = await this.roleService.AddRole(roleData.Name);
            if(!addResult)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete("role")]
        public async Task<IActionResult> RemoveRole([FromBody] RoleData roleData)
        {
            var removeResult = await this.roleService.RemoveRole(roleData.Name);
            if(!removeResult)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("user/role")]
        public async Task<IActionResult> AddRoleToUser([FromBody] AddRoleToUserModel model)
        {
            var addRoleResult = await this.authService.AddRoleToUser(model);

            if (addRoleResult)
                return Ok();

            return BadRequest();
        }
    }
}
