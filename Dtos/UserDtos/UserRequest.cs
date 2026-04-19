using Multi_Tenant_E_Commerce_API.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Multi_Tenant_E_Commerce_API.Dtos.UserDtos
{
    public class UserRequest
    {
        [Required] public UserRoleEnum Role { get; set; }
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public string Email { get; set; }
        [Required] public string Password { get; set; }
        public string? PhoneNumber { get; set; }
        public Guid? CompanyId { get; set; }
        public IFormFile? Image { get; set; }
    }
}
