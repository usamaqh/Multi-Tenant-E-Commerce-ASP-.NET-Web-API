using System.ComponentModel.DataAnnotations;

namespace Multi_Tenant_E_Commerce_API.Dtos.ItemDtos
{
    public class ItemRequest
    {
        [Required] public Guid CompanyId { get; set; }
        [Required] public string Name { get; set; }
        public IFormFile? Image { get; set; }
        [Required] public int InStock { get; set; }
        [Required] public decimal Price { get; set; }
    }
}
