using Multi_Tenant_E_Commerce_API.Dtos.ItemDtos;

namespace Multi_Tenant_E_Commerce_API.Dtos.CompanyDtos
{
    public class CompanyFinanceDashboardResponse
    {
        public decimal TotalRevenue { get; set; }
        public int ItemsCount { get; set; }
        public ICollection<ItemForCompanyFinanceResponse> Items { get; set; }
    }
}
