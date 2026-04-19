using System.ComponentModel.DataAnnotations;

namespace Multi_Tenant_E_Commerce_API.Dtos.OrderDtos
{
    public class OrderItemRequest
    {
        [Required] public string ItemName { get; set; }
        public string? ItemImageURL { get; set; }
        [Required] public decimal ItemPrice { get; set; }
        [Required] public int Quantity { get; set; }
    }
}
