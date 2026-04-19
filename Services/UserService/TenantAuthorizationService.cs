using Microsoft.EntityFrameworkCore;
using Multi_Tenant_E_Commerce_API.Data;
using Multi_Tenant_E_Commerce_API.Helpers;

namespace Multi_Tenant_E_Commerce_API.Services.Authorization
{
    public class TenantAuthorizationService : ITenantAuthorizationService
    {
        private readonly AppDbContext _dbContext;

        public TenantAuthorizationService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> UserBelongsToCompany(Guid userId, Guid companyId)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .AnyAsync(c =>
                    !c.IsDeleted &&
                    c.UserId == userId &&
                    c.CompanyId == companyId);
        }

        public async Task<bool> CanAccessCompany(Guid userId, UserRoleEnum role, Guid companyId)
        {
            if (role == UserRoleEnum.SuperAdmin)
                return true;

            if (role == UserRoleEnum.StoreAdmin)
                return await UserBelongsToCompany(userId, companyId);

            return false;
        }
    }
}