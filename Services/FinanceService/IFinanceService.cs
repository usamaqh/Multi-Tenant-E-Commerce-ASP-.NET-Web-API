using Multi_Tenant_E_Commerce_API.Dtos.CompanyDtos;
using Multi_Tenant_E_Commerce_API.Dtos.UserDtos;

namespace Multi_Tenant_E_Commerce_API.Services.FinanceService
{
    public interface IFinanceService
    {
        public Task<CustomerPurchaseHistoryResponse> GetCustomerPurchaseHistory(Guid userId);
        public Task<CompanyFinanceDashboardResponse> GetCompanyFinanceDashboard(Guid companyId);
    }
}
