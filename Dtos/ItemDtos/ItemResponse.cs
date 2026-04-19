using Multi_Tenant_E_Commerce_API.Dtos.CompanyDtos;
using Multi_Tenant_E_Commerce_API.Dtos.UserDtos;

namespace Multi_Tenant_E_Commerce_API.Dtos.ItemDtos
{
    public class ItemResponse
    {
        public Guid ItemId { get; set; } // search key
        public string Name { get; set; }
        public string? ImagePath { get; set; }
        public int InStock { get; set; }
        public decimal Price { get; set; }
        public int SoldUnits { get; set; }
        public decimal Revenue { get; set; }
        public DateTime CreatedAt { get; set; }

        public CompanyResponse? Company { get; set; }

        public UserResponse? AddedByUser { get; set; }
    }
}
