namespace Multi_Tenant_E_Commerce_API.Dtos.CartDtos
{
    public class CartResponse
    {
        public Guid CartId { get; set; } // search key
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<CartItemResponse>? CartItems { get; set; }
    }
}
