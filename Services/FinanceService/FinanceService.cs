using Microsoft.EntityFrameworkCore;
using Multi_Tenant_E_Commerce_API.Data;
using Multi_Tenant_E_Commerce_API.Dtos.CompanyDtos;
using Multi_Tenant_E_Commerce_API.Dtos.ItemDtos;
using Multi_Tenant_E_Commerce_API.Dtos.OrderDtos;
using Multi_Tenant_E_Commerce_API.Dtos.UserDtos;
using Multi_Tenant_E_Commerce_API.Models;

namespace Multi_Tenant_E_Commerce_API.Services.FinanceService
{
    public class FinanceService : IFinanceService
    {
        private AppDbContext _dbContext;

        public FinanceService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CustomerPurchaseHistoryResponse> GetCustomerPurchaseHistory(Guid userId)
        {
            List<OrderResponse> ordersList = await _dbContext.Orders
            .AsNoTracking()
            .Where(o => !o.IsDeleted && o.Customer.UserId == userId)
            .Select(o => new OrderResponse
            {
                TotalPrice = o.TotalPrice,
                PaidAt = o.PaidAt,
                PurchasedItems = _dbContext.OrderItems
                    .Where(oi => oi.OrderId == o.Id)
                    .Select(oi => new OrderItemRequest
                    {
                        ItemName = oi.ItemName,
                        ItemImageURL = oi.ItemImageURL,
                        ItemPrice = oi.ItemPrice,
                        Quantity = oi.Quantity
                    })
                    .ToList()
            })
            .ToListAsync();

            decimal totalExpenditure = ordersList.Sum(o => o.TotalPrice);

            return new CustomerPurchaseHistoryResponse
            {
                TotalExpenditure = totalExpenditure,
                PurchasedOrders = ordersList
            };
        }

        public async Task<CompanyFinanceDashboardResponse> GetCompanyFinanceDashboard(Guid companyId)
        {
            List<ItemForCompanyFinanceResponse> items = await _dbContext.Items
            .AsNoTracking()
            .Where(x => !x.IsDeleted && x.Company.CompanyId == companyId)
            .Select(x => new ItemForCompanyFinanceResponse
            {
                Name = x.Name,
                Price = x.Price,
                InStock = x.InStock,
                Revenue = x.Revenue,
                SoldUnits = x.SoldUnits
            })
            .ToListAsync();

            decimal total = items.Sum(x => x.Revenue);

            return new CompanyFinanceDashboardResponse
            {
                TotalRevenue = total,
                ItemsCount = items.Count,
                Items = items
            };
        }
    }
}
