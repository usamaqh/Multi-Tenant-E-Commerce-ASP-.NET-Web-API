using Multi_Tenant_E_Commerce_API.Dtos.CompanyDtos;

namespace Multi_Tenant_E_Commerce_API.Dtos.ItemDtos
{
    public class ItemForCartResponse
    {
        public Guid ItemId { get; set; } // search key
        public string Name { get; set; }
        public string? ImagePath { get; set; }
        public int InStock { get; set; }
        public decimal Price { get; set; }

        public CompanyForCartItemResponse ItemCompany { get; set; }
    }
}
