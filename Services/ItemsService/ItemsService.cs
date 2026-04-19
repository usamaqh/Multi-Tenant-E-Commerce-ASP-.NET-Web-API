using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Multi_Tenant_E_Commerce_API.Data;
using Multi_Tenant_E_Commerce_API.Dtos.CompanyDtos;
using Multi_Tenant_E_Commerce_API.Dtos.ItemDtos;
using Multi_Tenant_E_Commerce_API.Dtos.UserDtos;
using Multi_Tenant_E_Commerce_API.Helpers;
using Multi_Tenant_E_Commerce_API.Models;

namespace Multi_Tenant_E_Commerce_API.Services.ItemsService
{
    public class ItemsService : IItemsService
    {
        private AppDbContext _dbContext;

        public ItemsService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ItemResponse>> GetAllItems(Guid companyId)
        {
            return await _dbContext.Items
                .AsNoTracking()
                .Where(i => !i.IsDeleted && i.Company.CompanyId == companyId)
                .Select(i => new ItemResponse
                {
                    ItemId = i.ItemId,
                    Name = i.Name,
                    Price = i.Price,
                    InStock = i.InStock,
                    SoldUnits = i.SoldUnits,
                    Revenue = i.Revenue,
                    ImagePath = i.ImagePath,
                    CreatedAt = i.CreatedAt,

                    Company = i.Company != null ? new CompanyResponse
                    {
                        CompanyId = i.Company.CompanyId,
                        Name = i.Company.Name,
                        Email = i.Company.Email,
                        Address = i.Company.Address,
                        PhoneNumber = i.Company.PhoneNumber,
                        Website = i.Company.Website,
                        ImagePath = i.Company.ImagePath,
                        CreatedAt = i.Company.CreatedAt
                    } : null,

                    AddedByUser = i.AddedByUser != null ? new UserResponse
                    {
                        UserId = i.AddedByUser.UserId,
                        UserRole = i.AddedByUser.Role,
                        FirstName = i.AddedByUser.FirstName,
                        LastName = i.AddedByUser.LastName,
                        Email = i.AddedByUser.Email,
                        PhoneNumber = i.AddedByUser.PhoneNumber,
                        ImagePath = i.AddedByUser.ImagePath,
                        CreatedAt = i.AddedByUser.CreatedAt
                    } : null
                })
                .ToListAsync();
        }

        public async Task<ItemResponse?> GetItemById(Guid companyId, Guid itemId)
        {
            return await _dbContext.Items
                .Where(i => !i.IsDeleted && i.Company.CompanyId == companyId && i.ItemId == itemId)
                .Select(i => new ItemResponse
                {
                    ItemId = i.ItemId,
                    Name = i.Name,
                    Price = i.Price,
                    InStock = i.InStock,
                    SoldUnits = i.SoldUnits,
                    Revenue = i.Revenue,
                    ImagePath = i.ImagePath,
                    CreatedAt = i.CreatedAt,

                    Company = i.Company != null ? new CompanyResponse
                    {
                        CompanyId = i.Company.CompanyId,
                        Name = i.Company.Name,
                        Email = i.Company.Email,
                        Address = i.Company.Address,
                        PhoneNumber = i.Company.PhoneNumber,
                        Website = i.Company.Website,
                        ImagePath = i.Company.ImagePath,
                        CreatedAt = i.Company.CreatedAt
                    } : null,

                    AddedByUser = i.AddedByUser != null ? new UserResponse
                    {
                        UserId = i.AddedByUser.UserId,
                        UserRole = i.AddedByUser.Role,
                        FirstName = i.AddedByUser.FirstName,
                        LastName = i.AddedByUser.LastName,
                        Email = i.AddedByUser.Email,
                        PhoneNumber = i.AddedByUser.PhoneNumber,
                        ImagePath = i.AddedByUser.ImagePath,
                        CreatedAt = i.AddedByUser.CreatedAt
                    } : null
                })
                .FirstOrDefaultAsync();
        }

        public async Task<ItemResponse?> GetItemByName(Guid companyId, string itemName)
        {
            return await _dbContext.Items
                .Where(i => !i.IsDeleted && i.Company.CompanyId == companyId && i.Name == itemName)
                .Select(i => new ItemResponse
                {
                    ItemId = i.ItemId,
                    Name = i.Name,
                    Price = i.Price,
                    InStock = i.InStock,
                    SoldUnits = i.SoldUnits,
                    Revenue = i.Revenue,
                    ImagePath = i.ImagePath,
                    CreatedAt = i.CreatedAt,

                    Company = i.Company != null ? new CompanyResponse
                    {
                        CompanyId = i.Company.CompanyId,
                        Name = i.Company.Name,
                        Email = i.Company.Email,
                        Address = i.Company.Address,
                        PhoneNumber = i.Company.PhoneNumber,
                        Website = i.Company.Website,
                        ImagePath = i.Company.ImagePath,
                        CreatedAt = i.Company.CreatedAt
                    } : null,

                    AddedByUser = i.AddedByUser != null ? new UserResponse
                    {
                        UserId = i.AddedByUser.UserId,
                        UserRole = i.AddedByUser.Role,
                        FirstName = i.AddedByUser.FirstName,
                        LastName = i.AddedByUser.LastName,
                        Email = i.AddedByUser.Email,
                        PhoneNumber = i.AddedByUser.PhoneNumber,
                        ImagePath = i.AddedByUser.ImagePath,
                        CreatedAt = i.AddedByUser.CreatedAt
                    } : null
                })
                .FirstOrDefaultAsync();
        }

        public async Task<ItemResponse?> AddItem(ItemRequest newItem, Guid? currentUserId)
        {
            Item? item = await _dbContext.Items
                .FirstOrDefaultAsync(i => i.Company.CompanyId == newItem.CompanyId && i.Name == newItem.Name);

            if (item != null)
                return null;

            Item toAddItem = new Item();
            toAddItem.Name = newItem.Name;
            toAddItem.Price = newItem.Price;
            toAddItem.InStock = newItem.InStock;
            toAddItem.ImagePath = newItem.Image != null ? await ImageUploadHelper.UploadImage(newItem.Image) : null;

            Company? company = await _dbContext.Companies.FirstOrDefaultAsync(c => c.CompanyId == newItem.CompanyId);

            if (company == null)
                return null;

            toAddItem.CompanyId = company.Id;

            User? user = await _dbContext.Users.FirstOrDefaultAsync(c => c.UserId == currentUserId);

            if (user == null)
                return null;

            toAddItem.UserId = user.Id;

            EntityEntry<Item> itemFromDb = await _dbContext.Items.AddAsync(toAddItem);
            await _dbContext.SaveChangesAsync();

            return new ItemResponse
            {
                ItemId = itemFromDb.Entity.ItemId,
                Name = itemFromDb.Entity.Name,
                Price = itemFromDb.Entity.Price,
                InStock = itemFromDb.Entity.InStock,
                SoldUnits = itemFromDb.Entity.SoldUnits,
                Revenue = itemFromDb.Entity.Revenue,
                ImagePath = itemFromDb.Entity.ImagePath,
                CreatedAt = itemFromDb.Entity.CreatedAt,

                Company = itemFromDb.Entity.Company != null ? new CompanyResponse
                {
                    CompanyId = itemFromDb.Entity.Company.CompanyId,
                    Name = itemFromDb.Entity.Company.Name,
                    Email = itemFromDb.Entity.Company.Email,
                    Address = itemFromDb.Entity.Company.Address,
                    PhoneNumber = itemFromDb.Entity.Company.PhoneNumber,
                    Website = itemFromDb.Entity.Company.Website,
                    ImagePath = itemFromDb.Entity.Company.ImagePath,
                    CreatedAt = itemFromDb.Entity.Company.CreatedAt
                } : null,

                AddedByUser = itemFromDb.Entity.AddedByUser != null ? new UserResponse
                {
                    UserId = itemFromDb.Entity.AddedByUser.UserId,
                    UserRole = itemFromDb.Entity.AddedByUser.Role,
                    FirstName = itemFromDb.Entity.AddedByUser.FirstName,
                    LastName = itemFromDb.Entity.AddedByUser.LastName,
                    Email = itemFromDb.Entity.AddedByUser.Email,
                    PhoneNumber = itemFromDb.Entity.AddedByUser.PhoneNumber,
                    ImagePath = itemFromDb.Entity.AddedByUser.ImagePath,
                    CreatedAt = itemFromDb.Entity.AddedByUser.CreatedAt
                } : null
            };
        }

        public async Task<bool> UpdateItem(Guid companyId, Guid itemId, ItemRequest newItem)
        {
            Item? item = await _dbContext.Items
                .FirstOrDefaultAsync(i => !i.IsDeleted && i.Company.CompanyId == companyId && i.ItemId == itemId);

            if (item == null)
                return false;

            item.Name = newItem.Name;
            item.Price = newItem.Price;
            item.InStock = newItem.InStock;
            item.ImagePath = newItem.Image != null ? await ImageUploadHelper.UploadImage(newItem.Image) : item.ImagePath;

            EntityEntry<Item> itemFromDb = _dbContext.Items.Update(item);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteItem(Guid companyId, Guid itemId)
        {
            Item? item = await _dbContext.Items
                .FirstOrDefaultAsync(i => !i.IsDeleted && i.Company.CompanyId == companyId && i.ItemId == itemId);

            if (item == null)
                return false;

            item.IsDeleted = true;

            _dbContext.Items.Update(item);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UnDeleteItem(Guid companyId, Guid itemId)
        {
            Item? item = await _dbContext.Items
                            .FirstOrDefaultAsync(i => i.IsDeleted && i.Company.CompanyId == companyId && i.ItemId == itemId);

            if (item == null)
                return false;

            item.IsDeleted = false;

            _dbContext.Items.Update(item);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
