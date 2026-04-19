using System.Security.Claims;

namespace Multi_Tenant_E_Commerce_API.Helpers
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetLoggedInUserRoleString(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Role)?.Value ?? "";
        }

        public static UserRoleEnum? GetLoggedInUserRole(this ClaimsPrincipal user)
        {
            string curUserRole = GetLoggedInUserRoleString(user);

            if (string.IsNullOrEmpty(curUserRole))
                return null;

            UserRoleEnum role = UserRoleEnum.SuperAdmin;

            if (curUserRole == UserRoleEnum.StoreAdmin.ToString())
                role = UserRoleEnum.StoreAdmin;
            else if (curUserRole == UserRoleEnum.Customer.ToString())
                role = UserRoleEnum.Customer;

            return (role);
        }

        public static Guid GetLoggedInUserId(this ClaimsPrincipal user)
        {
            var userId = user.FindFirst("UserId")?.Value;
            return userId != null ? Guid.Parse(userId) : Guid.Empty;
        }

        public static bool CheckCurrentRole(this ClaimsPrincipal user, UserRoleEnum userRole)
        {
            return GetLoggedInUserRoleString(user) == userRole.ToString();
        }
    }
}
