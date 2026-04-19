using System.ComponentModel.DataAnnotations;

namespace Multi_Tenant_E_Commerce_API.Dtos.CartDtos
{
    public class CartItemRequest
    {
        [Required] public Guid ItemId { get; set; }
        [Required] public int Quantity { get; set; }
    }
}
