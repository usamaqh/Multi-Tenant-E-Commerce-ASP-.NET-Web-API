using System.ComponentModel.DataAnnotations;

namespace Multi_Tenant_E_Commerce_API.Dtos.CompanyDtos
{
    public class CompanyRequest
    {
        [Required] public string Name { get; set; }
        [Required] public string Email { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Website { get; set; }
        public IFormFile? Image { get; set; }
    }
}
