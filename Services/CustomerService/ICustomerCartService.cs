using Multi_Tenant_E_Commerce_API.Dtos.CartDtos;
using Multi_Tenant_E_Commerce_API.Dtos.OrderDtos;

namespace Multi_Tenant_E_Commerce_API.Services.CustomerService
{
    public interface ICustomerCartService
    {
        public Task<CartResponse?> GetCartDetails(Guid userId);
        public Task<(string, CartResponse?)> AddItemToCart(Guid userId, CartItemRequest newItem);
        public Task<(string, CartResponse?)> UpdateCartItem(Guid userId, CartItemRequest newItem);
        public Task<(string, CartResponse?)> RemoveCartItem(Guid userId, Guid itemId);
        public Task<bool> RemoveCart(Guid userId);
        public Task<OrderResponse?> CheckOutCart(Guid userId);
    }
}
