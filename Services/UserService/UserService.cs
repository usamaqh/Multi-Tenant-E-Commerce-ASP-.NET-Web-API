using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Multi_Tenant_E_Commerce_API.Data;
using Multi_Tenant_E_Commerce_API.Dtos.CompanyDtos;
using Multi_Tenant_E_Commerce_API.Dtos.UserDtos;
using Multi_Tenant_E_Commerce_API.Helpers;
using Multi_Tenant_E_Commerce_API.Models;

namespace Multi_Tenant_E_Commerce_API.Services.UserService
{
    public class UserService : IUserService
    {
        private AppDbContext _dbContext;

        public UserService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<UserResponse>> GetAllUsers(UserRoleEnum? currentRole, Guid currentUserId)
        {
            IQueryable<User> query = _dbContext.Users
                .AsNoTracking()
                .Where(x => !x.IsDeleted);

            // if store admin then get all users from the same company only
            if (currentRole == UserRoleEnum.StoreAdmin)
            {
                User? user = await _dbContext.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.UserId == currentUserId);

                if (user == null)
                    return new List<UserResponse>();

                Guid companyId = user.CompanyId;

                query = query.Where(u => u.CompanyId == companyId);
            }

            return await query
                    .Select(u => new UserResponse
                    {
                        UserId = u.UserId,
                        UserRole = u.Role,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Email = u.Email,
                        PhoneNumber = u.PhoneNumber,
                        ImagePath = u.ImagePath,
                        CreatedAt = u.CreatedAt,

                        CompaniesAdded = u.Companies != null ? u.Companies.Select(c => new CompanyResponse
                        {
                            CompanyId = c.CompanyId,
                            Name = c.Name,
                            Email = c.Email,
                            Address = c.Address,
                            PhoneNumber = c.PhoneNumber,
                            Website = c.Website,
                            ImagePath = c.ImagePath,
                            CreatedAt = c.CreatedAt
                        }).ToList() : new List<CompanyResponse>(),

                        CreatedByUser = u.CreatedByUser != null ? new UserResponse
                        {
                            UserId = u.CreatedByUser.UserId,
                            UserRole = u.CreatedByUser.Role,
                            FirstName = u.CreatedByUser.FirstName,
                            LastName = u.CreatedByUser.LastName,
                            Email = u.CreatedByUser.Email,
                            PhoneNumber = u.CreatedByUser.PhoneNumber,
                            ImagePath = u.CreatedByUser.ImagePath,
                            CreatedAt = u.CreatedByUser.CreatedAt
                        } : null
                    })
                    .ToListAsync();
        }

        public async Task<List<UserResponse>> GetAllUsersByRole(UserRoleEnum? currentRole, Guid currentUserId, UserRoleEnum userRole)
        {
            IQueryable<User> query = _dbContext.Users
                .AsNoTracking()
                .Where(x => !x.IsDeleted && x.Role == userRole);

            // if store admin then get users from the same company only
            if (currentRole == UserRoleEnum.StoreAdmin)
            {
                User? user = await _dbContext.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.UserId == currentUserId);

                if (user == null)
                    return new List<UserResponse>();

                Guid companyId = user.CompanyId;

                query = query.Where(u => u.CompanyId == companyId);
            }

            return await query
                    .Select(u => new UserResponse
                    {
                        UserId = u.UserId,
                        UserRole = u.Role,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Email = u.Email,
                        PhoneNumber = u.PhoneNumber,
                        ImagePath = u.ImagePath,
                        CreatedAt = u.CreatedAt,

                        CompaniesAdded = u.Companies != null ? u.Companies.Select(c => new CompanyResponse
                        {
                            CompanyId = c.CompanyId,
                            Name = c.Name,
                            Email = c.Email,
                            Address = c.Address,
                            PhoneNumber = c.PhoneNumber,
                            Website = c.Website,
                            ImagePath = c.ImagePath,
                            CreatedAt = c.CreatedAt
                        }).ToList() : new List<CompanyResponse>(),

                        CreatedByUser = u.CreatedByUser != null ? new UserResponse
                        {
                            UserId = u.CreatedByUser.UserId,
                            UserRole = u.CreatedByUser.Role,
                            FirstName = u.CreatedByUser.FirstName,
                            LastName = u.CreatedByUser.LastName,
                            Email = u.CreatedByUser.Email,
                            PhoneNumber = u.CreatedByUser.PhoneNumber,
                            ImagePath = u.CreatedByUser.ImagePath,
                            CreatedAt = u.CreatedByUser.CreatedAt
                        } : null
                    })
                    .ToListAsync();
        }

        public async Task<UserResponse?> GetUserByEmail(UserRoleEnum? currentRole, Guid currentUserId, string userEmail)
        {
            IQueryable<User> query = _dbContext.Users
                .AsNoTracking()
                .Where(x => !x.IsDeleted && x.Email == userEmail);

            // if store admin then get user from the same company only
            // if customer then get their user only
            if (currentRole == UserRoleEnum.Customer)
                query = query.Where(u => u.UserId == currentUserId);
            else if (currentRole == UserRoleEnum.StoreAdmin)
            {
                User? user = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == currentUserId);

                if (user == null)
                    return null;

                Guid companyId = user.CompanyId;
                query = query.Where(u => u.CompanyId == companyId);
            }

            return await query
                    .Select(u => new UserResponse
                    {
                        UserId = u.UserId,
                        UserRole = u.Role,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Email = u.Email,
                        PhoneNumber = u.PhoneNumber,
                        ImagePath = u.ImagePath,
                        CreatedAt = u.CreatedAt,

                        CompaniesAdded = u.Companies != null ? u.Companies.Select(c => new CompanyResponse
                        {
                            CompanyId = c.CompanyId,
                            Name = c.Name,
                            Email = c.Email,
                            Address = c.Address,
                            PhoneNumber = c.PhoneNumber,
                            Website = c.Website,
                            ImagePath = c.ImagePath,
                            CreatedAt = c.CreatedAt
                        }).ToList() : new List<CompanyResponse>(),

                        CreatedByUser = u.CreatedByUser != null ? new UserResponse
                        {
                            UserId = u.CreatedByUser.UserId,
                            UserRole = u.CreatedByUser.Role,
                            FirstName = u.CreatedByUser.FirstName,
                            LastName = u.CreatedByUser.LastName,
                            Email = u.CreatedByUser.Email,
                            PhoneNumber = u.CreatedByUser.PhoneNumber,
                            ImagePath = u.CreatedByUser.ImagePath,
                            CreatedAt = u.CreatedByUser.CreatedAt
                        } : null
                    })
                    .FirstOrDefaultAsync();
        }

        public async Task<UserResponse?> GetUserByUserId(UserRoleEnum? currentRole, Guid currentUserId, Guid userId)
        {
            IQueryable<User> query = _dbContext.Users
                .AsNoTracking()
                .Where(x => !x.IsDeleted && x.UserId == userId);

            // if store admin then get user from the same company only
            if (currentRole == UserRoleEnum.StoreAdmin)
            {
                User? user = await _dbContext.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.UserId == currentUserId);

                if (user == null)
                    return null;

                Guid companyId = user.CompanyId;

                query = query.Where(u => u.CompanyId == companyId);
            }

            return await query
                    .Select(u => new UserResponse
                    {
                        UserId = u.UserId,
                        UserRole = u.Role,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Email = u.Email,
                        PhoneNumber = u.PhoneNumber,
                        ImagePath = u.ImagePath,
                        CreatedAt = u.CreatedAt,

                        CompaniesAdded = u.Companies != null ? u.Companies.Select(c => new CompanyResponse
                        {
                            CompanyId = c.CompanyId,
                            Name = c.Name,
                            Email = c.Email,
                            Address = c.Address,
                            PhoneNumber = c.PhoneNumber,
                            Website = c.Website,
                            ImagePath = c.ImagePath,
                            CreatedAt = c.CreatedAt
                        }).ToList() : new List<CompanyResponse>(),

                        CreatedByUser = u.CreatedByUser != null ? new UserResponse
                        {
                            UserId = u.CreatedByUser.UserId,
                            UserRole = u.CreatedByUser.Role,
                            FirstName = u.CreatedByUser.FirstName,
                            LastName = u.CreatedByUser.LastName,
                            Email = u.CreatedByUser.Email,
                            PhoneNumber = u.CreatedByUser.PhoneNumber,
                            ImagePath = u.CreatedByUser.ImagePath,
                            CreatedAt = u.CreatedByUser.CreatedAt
                        } : null
                    })
                    .FirstOrDefaultAsync();
        }

        public async Task<UserResponse?> GetUserByName(UserRoleEnum? currentRole, Guid currentUserId, string fName, string lName)
        {
            IQueryable<User> query = _dbContext.Users
                .AsNoTracking()
                .Where(x => !x.IsDeleted && (x.FirstName == fName && x.LastName == lName));

            // if store admin then get user from the same company only
            if (currentRole == UserRoleEnum.StoreAdmin)
            {
                User? user = await _dbContext.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.UserId == currentUserId);

                if (user == null)
                    return null;

                Guid companyId = user.CompanyId;

                query = query.Where(u => u.CompanyId == companyId);
            }

            return await query
                    .Select(u => new UserResponse
                    {
                        UserId = u.UserId,
                        UserRole = u.Role,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Email = u.Email,
                        PhoneNumber = u.PhoneNumber,
                        ImagePath = u.ImagePath,
                        CreatedAt = u.CreatedAt,

                        CompaniesAdded = u.Companies != null ? u.Companies.Select(c => new CompanyResponse
                        {
                            CompanyId = c.CompanyId,
                            Name = c.Name,
                            Email = c.Email,
                            Address = c.Address,
                            PhoneNumber = c.PhoneNumber,
                            Website = c.Website,
                            ImagePath = c.ImagePath,
                            CreatedAt = c.CreatedAt
                        }).ToList() : new List<CompanyResponse>(),

                        CreatedByUser = u.CreatedByUser != null ? new UserResponse
                        {
                            UserId = u.CreatedByUser.UserId,
                            UserRole = u.CreatedByUser.Role,
                            FirstName = u.CreatedByUser.FirstName,
                            LastName = u.CreatedByUser.LastName,
                            Email = u.CreatedByUser.Email,
                            PhoneNumber = u.CreatedByUser.PhoneNumber,
                            ImagePath = u.CreatedByUser.ImagePath,
                            CreatedAt = u.CreatedByUser.CreatedAt
                        } : null
                    })
                    .FirstOrDefaultAsync();
        }

        public async Task<UserResponse?> TestAddUser(UserRequest newUserRequest)
        {
            User user = new User();
            user.Role = newUserRequest.Role;
            user.FirstName = newUserRequest.FirstName;
            user.LastName = newUserRequest.LastName;
            user.Email = newUserRequest.Email;
            user.PasswordHash = PasswordHasherHelper.HashPassword(user, newUserRequest.Password);
            user.PhoneNumber = newUserRequest.PhoneNumber;
            user.ImagePath = newUserRequest.Image != null ? await ImageUploadHelper.UploadImage(newUserRequest.Image) : null;

            EntityEntry<User> addedUser = await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return new UserResponse
            {
                UserId = addedUser.Entity.UserId,
                UserRole = addedUser.Entity.Role,
                FirstName = addedUser.Entity.FirstName,
                LastName = addedUser.Entity.LastName,
                Email = addedUser.Entity.Email,
                PhoneNumber = addedUser.Entity.PhoneNumber,
                ImagePath = addedUser.Entity.ImagePath,
                CreatedAt = addedUser.Entity.CreatedAt,

                CompaniesAdded = addedUser.Entity.Companies != null ? addedUser.Entity.Companies.Select(c => new CompanyResponse
                {
                    CompanyId = c.CompanyId,
                    Name = c.Name,
                    Email = c.Email,
                    Address = c.Address,
                    PhoneNumber = c.PhoneNumber,
                    Website = c.Website,
                    ImagePath = c.ImagePath,
                    CreatedAt = c.CreatedAt
                }).ToList() : new List<CompanyResponse>(),

                CreatedByUser = addedUser.Entity.CreatedByUser != null ? new UserResponse
                {
                    UserId = addedUser.Entity.CreatedByUser.UserId,
                    UserRole = addedUser.Entity.CreatedByUser.Role,
                    FirstName = addedUser.Entity.CreatedByUser.FirstName,
                    LastName = addedUser.Entity.CreatedByUser.LastName,
                    Email = addedUser.Entity.CreatedByUser.Email,
                    PhoneNumber = addedUser.Entity.CreatedByUser.PhoneNumber,
                    ImagePath = addedUser.Entity.CreatedByUser.ImagePath,
                    CreatedAt = addedUser.Entity.CreatedByUser.CreatedAt
                } : null
            };
        }

        public async Task<UserResponse?> AddUser(UserRequest newUserRequest, UserRoleEnum? currentRole, Guid? currentUserId)
        {
            User? alreadyUser = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == newUserRequest.Email);

            if (alreadyUser != null)
                return null;

            User user = new User();
            user.FirstName = newUserRequest.FirstName;
            user.LastName = newUserRequest.LastName;
            user.PhoneNumber = newUserRequest.PhoneNumber;
            user.Email = newUserRequest.Email;
            user.Role = newUserRequest.Role;
            user.PasswordHash = PasswordHasherHelper.HashPassword(user, newUserRequest.Password);
            user.ImagePath = newUserRequest.Image != null ? await ImageUploadHelper.UploadImage(newUserRequest.Image) : null;
            user.CompanyId = newUserRequest.CompanyId ?? Guid.Empty;

            User? createdByUser = await _dbContext.Users
                .FirstOrDefaultAsync(u => !u.IsDeleted && u.UserId == currentUserId);

            if (createdByUser == null)
                return null;

            user.CreatedByUserId = createdByUser.Id;

            EntityEntry<User> addedUser = await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return new UserResponse
            {
                UserId = addedUser.Entity.UserId,
                UserRole = addedUser.Entity.Role,
                FirstName = addedUser.Entity.FirstName,
                LastName = addedUser.Entity.LastName,
                Email = addedUser.Entity.Email,
                PhoneNumber = addedUser.Entity.PhoneNumber,
                ImagePath = addedUser.Entity.ImagePath,
                CreatedAt = addedUser.Entity.CreatedAt,

                CompaniesAdded = addedUser.Entity.Companies != null ? addedUser.Entity.Companies.Select(c => new CompanyResponse
                {
                    CompanyId = c.CompanyId,
                    Name = c.Name,
                    Email = c.Email,
                    Address = c.Address,
                    PhoneNumber = c.PhoneNumber,
                    Website = c.Website,
                    ImagePath = c.ImagePath,
                    CreatedAt = c.CreatedAt
                }).ToList() : new List<CompanyResponse>(),

                CreatedByUser = addedUser.Entity.CreatedByUser != null ? new UserResponse
                {
                    UserId = addedUser.Entity.CreatedByUser.UserId,
                    UserRole = addedUser.Entity.CreatedByUser.Role,
                    FirstName = addedUser.Entity.CreatedByUser.FirstName,
                    LastName = addedUser.Entity.CreatedByUser.LastName,
                    Email = addedUser.Entity.CreatedByUser.Email,
                    PhoneNumber = addedUser.Entity.CreatedByUser.PhoneNumber,
                    ImagePath = addedUser.Entity.CreatedByUser.ImagePath,
                    CreatedAt = addedUser.Entity.CreatedByUser.CreatedAt
                } : null
            };
        }

        public async Task<bool> UpdateUser(Guid userID, UserRequest userRequest, UserRoleEnum? currentRole, Guid? currentUserId)
        {
            User? user = await _dbContext.Users
                .FirstOrDefaultAsync(u => !u.IsDeleted && u.UserId == userID);

            if (user == null)
                return false;

            if (currentRole == UserRoleEnum.Customer && user.UserId != currentUserId)
                return false;

            user.FirstName = userRequest.FirstName;
            user.LastName = userRequest.LastName;
            user.Email = userRequest.Email;
            user.PhoneNumber = userRequest.PhoneNumber;
            user.PasswordHash = PasswordHasherHelper.HashPassword(user, userRequest.Password);
            user.ImagePath = userRequest.Image != null ? await ImageUploadHelper.UploadImage(userRequest.Image) : user.ImagePath;

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteUser(Guid userIdToDelete, UserRoleEnum? currentRole, Guid? currentUserId)
        {
            IQueryable<User> query = _dbContext.Users
                .Where(u => !u.IsDeleted && u.UserId == userIdToDelete);

            if (currentRole == UserRoleEnum.StoreAdmin)
            {
                User? user = await _dbContext.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.UserId == currentUserId);

                if (user == null)
                    return false;

                Guid companyId = user.CompanyId;
                query = query.Where(x => x.CompanyId == companyId);
            }

            User? userToDel = await query.FirstOrDefaultAsync();

            if (userToDel == null)
                return false;

            userToDel.IsDeleted = true;

            _dbContext.Users.Update(userToDel);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UnDeleteUser(Guid userId)
        {
            User? user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.IsDeleted && u.UserId == userId);

            if (user == null)
                return false;

            user.IsDeleted = false;

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
