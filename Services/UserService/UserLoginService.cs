using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Multi_Tenant_E_Commerce_API.Data;
using Multi_Tenant_E_Commerce_API.Dtos.UserDtos;
using Multi_Tenant_E_Commerce_API.Helpers;
using Multi_Tenant_E_Commerce_API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Multi_Tenant_E_Commerce_API.Services.UserService
{
    public class UserLoginService : IUserLoginService
    {
        private AppDbContext _dbContext;
        private IConfiguration _configuration;

        public UserLoginService(AppDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<(string?, UserResponse?)> UserLogin(string email, string password)
        {
            User? user = await _dbContext.Users
                .FirstOrDefaultAsync(u => !u.IsDeleted && u.Email == email);

            if (user == null)
                return ("User not found!", null);

            if (!PasswordHasherHelper.VerifyHashPassword(user, password))
                return ("Incorrect password!", null);


            string tokenVal = "Bearer " + GetToken(user);

            return (tokenVal, new UserResponse
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserRole = user.Role
            });
        }

        private string GetToken(User user)
        {
            Claim[] claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserId", user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            SigningCredentials signin = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: signin
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
