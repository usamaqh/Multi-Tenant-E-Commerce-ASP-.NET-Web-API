using Multi_Tenant_E_Commerce_API.Dtos.ItemDtos;

namespace Multi_Tenant_E_Commerce_API.Services.ItemsService
{
    public interface IItemsService
    {
        public Task<List<ItemResponse>> GetAllItems(Guid companyId);
        public Task<ItemResponse?> GetItemById(Guid companyId, Guid itemId);
        public Task<ItemResponse?> GetItemByName(Guid companyId, string itemName);

        public Task<ItemResponse?> AddItem(ItemRequest newItem, Guid? currentUserId);
        public Task<bool> UpdateItem(Guid companyId, Guid itemId, ItemRequest newItem);

        public Task<bool> DeleteItem(Guid companyId, Guid itemId);
        public Task<bool> UnDeleteItem(Guid companyId, Guid itemId);
    }
}
