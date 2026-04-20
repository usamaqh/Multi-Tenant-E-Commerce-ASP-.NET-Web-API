using Microsoft.AspNetCore.Mvc;
using Multi_Tenant_E_Commerce_API.Dtos.UserDtos;
using Multi_Tenant_E_Commerce_API.Helpers;
using Multi_Tenant_E_Commerce_API.Services.UserService;

namespace Multi_Tenant_E_Commerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : ControllerBase
    {
        private IUserLoginService _IUserLogin;

        public UserLoginController(IUserLoginService userLogin)
        {
            _IUserLogin = userLogin;
        }

        [HttpPost]
        [Route("login/{email}_{password}")]
        public async Task<IActionResult> UserLogin(string email, string password)
        {
            try
            {
                (string?, UserResponse?) result = await _IUserLogin.UserLogin(email, password);

                if (result.Item2 == null)
                    return BadRequest(result.Item1);

                return Ok(new { token = result.Item1, result.Item2 });
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
