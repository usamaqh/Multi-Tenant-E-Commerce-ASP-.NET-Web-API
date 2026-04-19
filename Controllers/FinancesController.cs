using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multi_Tenant_E_Commerce_API.Dtos.CompanyDtos;
using Multi_Tenant_E_Commerce_API.Dtos.UserDtos;
using Multi_Tenant_E_Commerce_API.Helpers;
using Multi_Tenant_E_Commerce_API.Services.Authorization;
using Multi_Tenant_E_Commerce_API.Services.FinanceService;

namespace Multi_Tenant_E_Commerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinancesController : ControllerBase
    {
        private IFinanceService _IFinanceService;
        private ITenantAuthorizationService _ITenantAuthorizationService;

        public FinancesController(IFinanceService IFinanceService, ITenantAuthorizationService tenantAuthorizationService)
        {
            _IFinanceService = IFinanceService;
            _ITenantAuthorizationService = tenantAuthorizationService;
        }

        [HttpGet]
        [Route("get_customer_purchase_history")]
        [Authorize(Roles = UserRolesHelper.SuperAdminRole + "," + UserRolesHelper.StoreAdminRole + "," + UserRolesHelper.CustomerRole)]
        public async Task<IActionResult> GetCustomerPurchaseHistory(Guid userId)
        {
            try
            {
                Guid currentUserId = User.CheckCurrentRole(UserRoleEnum.Customer) ? User.GetLoggedInUserId() : userId;

                CustomerPurchaseHistoryResponse result = await _IFinanceService.GetCustomerPurchaseHistory(currentUserId);

                if (result == null)
                    return NotFound("No data present!");

                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet]
        [Route("get_company_finance_dashboard/{companyId}")]
        [Authorize(Roles = UserRolesHelper.SuperAdminRole + "," + UserRolesHelper.StoreAdminRole)]
        public async Task<IActionResult> GetCompanyFinanceDashboard(Guid companyId)
        {
            try
            {
                Guid currentUserId = User.GetLoggedInUserId();
                UserRoleEnum? currentRole = User.GetLoggedInUserRole();

                if (currentUserId == Guid.Empty || currentRole == null)
                    return Unauthorized();

                if (!await _ITenantAuthorizationService.CanAccessCompany(currentUserId, currentRole.Value, companyId))
                    return Forbid();

                CompanyFinanceDashboardResponse result = await _IFinanceService.GetCompanyFinanceDashboard(companyId);

                if (result == null)
                    return NotFound("No data present!");

                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
