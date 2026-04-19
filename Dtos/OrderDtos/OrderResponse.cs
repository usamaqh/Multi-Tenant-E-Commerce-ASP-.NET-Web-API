using System.ComponentModel.DataAnnotations;

namespace Multi_Tenant_E_Commerce_API.Dtos.OrderDtos
{
    public class OrderResponse
    {
        public decimal TotalPrice { get; set; }
        public DateTime PaidAt { get; set; }

        public ICollection<OrderItemRequest> PurchasedItems { get; set; }
    }
}
