using Microsoft.EntityFrameworkCore;
using Multi_Tenant_E_Commerce_API.Data;
using Multi_Tenant_E_Commerce_API.Dtos.CompanyDtos;
using Multi_Tenant_E_Commerce_API.Dtos.UserDtos;
using Multi_Tenant_E_Commerce_API.Helpers;
using Multi_Tenant_E_Commerce_API.Models;

namespace Multi_Tenant_E_Commerce_API.Services.CompanyService
{
    public class CompanyService : ICompanyService
    {
        private AppDbContext _dbContext;

        public CompanyService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<CompanyResponse>> GetAllCompanies()
        {
            return await _dbContext.Companies
                .AsNoTracking()
                .Where(c => !c.IsDeleted)
                .Select(c => new CompanyResponse
                {
                    CompanyId = c.CompanyId,
                    Name = c.Name,
                    Email = c.Email,
                    Address = c.Address,
                    PhoneNumber = c.PhoneNumber,
                    Website = c.Website,
                    ImagePath = c.ImagePath,
                    CreatedAt = c.CreatedAt,

                    CreatedByUser = c.CreatedBy == null ? null : new UserMinimalResponse
                    {
                        UserRole = c.CreatedBy.Role,
                        FirstName = c.CreatedBy.FirstName,
                        LastName = c.CreatedBy.LastName
                    }
                })
                .ToListAsync();
        }

        public async Task<CompanyResponse?> GetCompanyById(Guid companyId)
        {
            return await _dbContext.Companies
                .Where(c => !c.IsDeleted && c.CompanyId == companyId)
                .AsNoTracking()
                .Select(c => new CompanyResponse
                {
                    CompanyId = c.CompanyId,
                    Name = c.Name,
                    Email = c.Email,
                    Address = c.Address,
                    PhoneNumber = c.PhoneNumber,
                    Website = c.Website,
                    ImagePath = c.ImagePath,
                    CreatedAt = c.CreatedAt,

                    CreatedByUser = c.CreatedBy == null ? null : new UserMinimalResponse
                    {
                        UserRole = c.CreatedBy.Role,
                        FirstName = c.CreatedBy.FirstName,
                        LastName = c.CreatedBy.LastName
                    }
                })
                .FirstOrDefaultAsync();
        }

        public async Task<CompanyResponse?> GetCompanyByName(string companyName)
        {
            return await _dbContext.Companies
                .Where(c => !c.IsDeleted && c.Name == companyName)
                .AsNoTracking()
                .Select(c => new CompanyResponse
                {
                    CompanyId = c.CompanyId,
                    Name = c.Name,
                    Email = c.Email,
                    Address = c.Address,
                    PhoneNumber = c.PhoneNumber,
                    Website = c.Website,
                    ImagePath = c.ImagePath,
                    CreatedAt = c.CreatedAt,

                    CreatedByUser = c.CreatedBy == null ? null : new UserMinimalResponse
                    {
                        UserRole = c.CreatedBy.Role,
                        FirstName = c.CreatedBy.FirstName,
                        LastName = c.CreatedBy.LastName
                    }
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<CompanyResponse>> GetCompanyByAdminId(Guid adminId)
        {
            return await _dbContext.Companies
                .Where(c => !c.IsDeleted && c.CreatedBy != null && c.CreatedBy.UserId == adminId)
                .AsNoTracking()
                .Select(c => new CompanyResponse
                {
                    CompanyId = c.CompanyId,
                    Name = c.Name,
                    Email = c.Email,
                    Address = c.Address,
                    PhoneNumber = c.PhoneNumber,
                    Website = c.Website,
                    ImagePath = c.ImagePath,
                    CreatedAt = c.CreatedAt,

                    CreatedByUser = c.CreatedBy == null ? null : new UserMinimalResponse
                    {
                        UserRole = c.CreatedBy.Role,
                        FirstName = c.CreatedBy.FirstName,
                        LastName = c.CreatedBy.LastName
                    }
                })
                .ToListAsync();
        }

        public async Task<CompanyResponse?> AddCompany(CompanyRequest newCompanyRequest, Guid adminUserId)
        {
            User? user = await _dbContext.Users
                .FirstOrDefaultAsync(u => !u.IsDeleted && u.UserId == adminUserId);

            if (user == null)
                return null;

            Company company = new Company
            {
                Name = newCompanyRequest.Name,
                Email = newCompanyRequest.Email,
                Address = newCompanyRequest.Address,
                PhoneNumber = newCompanyRequest.PhoneNumber,
                Website = newCompanyRequest.Website,
                ImagePath = newCompanyRequest.Image != null
                    ? await ImageUploadHelper.UploadImage(newCompanyRequest.Image)
                    : null,
                AdminId = user.Id
            };

            await _dbContext.Companies.AddAsync(company);
            await _dbContext.SaveChangesAsync();

            return new CompanyResponse
            {
                CompanyId = company.CompanyId,
                Name = company.Name,
                Email = company.Email,
                Address = company.Address,
                PhoneNumber = company.PhoneNumber,
                Website = company.Website,
                ImagePath = company.ImagePath,
                CreatedAt = company.CreatedAt,

                CreatedByUser = new UserMinimalResponse
                {
                    UserRole = user.Role,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                }
            };
        }

        public async Task<bool> UpdateCompany(Guid companyId, CompanyRequest updateCompanyRequest)
        {
            Company? company = await _dbContext.Companies.FirstOrDefaultAsync(c => !c.IsDeleted && c.CompanyId == companyId);

            if (company == null)
                return false;

            company.Name = updateCompanyRequest.Name;
            company.Email = updateCompanyRequest.Email;
            company.Address = updateCompanyRequest.Address;
            company.PhoneNumber = updateCompanyRequest.PhoneNumber;
            company.Website = updateCompanyRequest.Website;
            company.ImagePath = updateCompanyRequest.Image != null ? await ImageUploadHelper.UploadImage(updateCompanyRequest.Image) : company.ImagePath;

            _dbContext.Companies.Update(company);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteCompany(Guid companyId)
        {
            Company? company = await _dbContext.Companies.FirstOrDefaultAsync(c => !c.IsDeleted && c.CompanyId == companyId);

            if (company == null)
                return false;

            company.IsDeleted = true;

            _dbContext.Companies.Update(company);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UnDeleteCompany(Guid companyId)
        {
            Company? company = await _dbContext.Companies.FirstOrDefaultAsync(c => c.IsDeleted && c.CompanyId == companyId);

            if (company == null)
                return false;

            company.IsDeleted = false;

            _dbContext.Companies.Update(company);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
