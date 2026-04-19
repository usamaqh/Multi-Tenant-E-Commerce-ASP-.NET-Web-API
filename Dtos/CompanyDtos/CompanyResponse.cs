using Multi_Tenant_E_Commerce_API.Dtos.UserDtos;

namespace Multi_Tenant_E_Commerce_API.Dtos.CompanyDtos
{
    public class CompanyResponse
    {
        public Guid CompanyId { get; set; } // search key
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Website { get; set; }
        public string? ImagePath { get; set; }
        public DateTime CreatedAt { get; set; }

        public UserMinimalResponse? CreatedByUser { get; set; }
    }
}
