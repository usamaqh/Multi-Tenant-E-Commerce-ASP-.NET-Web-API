using Multi_Tenant_E_Commerce_API.Dtos.OrderDtos;

namespace Multi_Tenant_E_Commerce_API.Dtos.UserDtos
{
    public class CustomerPurchaseHistoryResponse
    {
        public decimal TotalExpenditure { get; set; }
        public ICollection<OrderResponse> PurchasedOrders { get; set; }
    }
}
