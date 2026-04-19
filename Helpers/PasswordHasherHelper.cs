using Microsoft.AspNetCore.Identity;
using Multi_Tenant_E_Commerce_API.Data;
using Multi_Tenant_E_Commerce_API.Models;

namespace Multi_Tenant_E_Commerce_API.Helpers
{
    public static class PasswordHasherHelper
    {
        private static readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        public static bool VerifyHashPassword(User user, string pass)
        {
            return _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, pass) == PasswordVerificationResult.Success;
        }

        public static string HashPassword(User user, string pass)
        {
            return _passwordHasher.HashPassword(user, pass);
        }
    }
}
