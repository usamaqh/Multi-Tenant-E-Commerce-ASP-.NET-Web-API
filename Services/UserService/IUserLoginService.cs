using Multi_Tenant_E_Commerce_API.Dtos.UserDtos;
using Multi_Tenant_E_Commerce_API.Helpers;

namespace Multi_Tenant_E_Commerce_API.Services.UserService
{
    public interface IUserLoginService
    {
        public Task<(string?, UserResponse?)> UserLogin(string email, string password);
    }
}
