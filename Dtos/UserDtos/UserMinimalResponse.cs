using Multi_Tenant_E_Commerce_API.Helpers;

namespace Multi_Tenant_E_Commerce_API.Dtos.UserDtos
{
    public class UserMinimalResponse
    {
        public UserRoleEnum UserRole { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
