using Multi_Tenant_E_Commerce_API.Dtos.CompanyDtos;
using Multi_Tenant_E_Commerce_API.Helpers;

namespace Multi_Tenant_E_Commerce_API.Dtos.UserDtos
{
    public class UserResponse
    {
        public Guid UserId { get; set; } // search key
        public UserRoleEnum UserRole { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ImagePath { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<CompanyResponse>? CompaniesAdded { get; set; }

        public UserResponse? CreatedByUser { get; set; }
    }
}
