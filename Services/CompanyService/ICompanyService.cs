using Multi_Tenant_E_Commerce_API.Dtos.CompanyDtos;

namespace Multi_Tenant_E_Commerce_API.Services.CompanyService
{
    public interface ICompanyService
    {
        public Task<List<CompanyResponse>> GetAllCompanies();
        public Task<CompanyResponse?> GetCompanyById(Guid companyId);
        public Task<CompanyResponse?> GetCompanyByName(string companyName);
        public Task<List<CompanyResponse>> GetCompanyByAdminId(Guid adminId);
        
        public Task<CompanyResponse?> AddCompany(CompanyRequest newCompanyRequest, Guid adminUserId);
        public Task<bool> UpdateCompany(Guid companyId, CompanyRequest updateCompanyRequest);
       
        public Task<bool> DeleteCompany(Guid companyId);
        public Task<bool> UnDeleteCompany(Guid companyId);
    }
}
