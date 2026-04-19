using Multi_Tenant_E_Commerce_API.Dtos.ItemDtos;

namespace Multi_Tenant_E_Commerce_API.Dtos.CartDtos
{
    public class CartItemResponse
    {
        public int Quantity { get; set; }

        public ItemForCartResponse Item { get; set; }
    }
}
