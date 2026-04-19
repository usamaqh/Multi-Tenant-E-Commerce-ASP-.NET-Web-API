using Multi_Tenant_E_Commerce_API.Helpers;

namespace Multi_Tenant_E_Commerce_API.Services.Authorization
{
    public interface ITenantAuthorizationService
    {
        Task<bool> UserBelongsToCompany(Guid userId, Guid companyId);
        Task<bool> CanAccessCompany(Guid userId, UserRoleEnum role, Guid companyId);
    }
}