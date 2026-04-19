using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multi_Tenant_E_Commerce_API.Dtos.ItemDtos;
using Multi_Tenant_E_Commerce_API.Helpers;
using Multi_Tenant_E_Commerce_API.Services.Authorization;
using Multi_Tenant_E_Commerce_API.Services.ItemsService;

namespace Multi_Tenant_E_Commerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private IItemsService _IItemsService;
        private ITenantAuthorizationService _ITenantAuthorizationService;

        public ItemsController(IItemsService itemsService, ITenantAuthorizationService tenantAuthorizationService)
        {
            _IItemsService = itemsService;
            _ITenantAuthorizationService = tenantAuthorizationService;
        }

        [HttpGet]
        [Route("get_items/{companyId}")]
        [Authorize(Roles = UserRolesHelper.SuperAdminRole + "," + UserRolesHelper.StoreAdminRole)]
        public async Task<IActionResult> GetAllItems(Guid companyId)
        {
            try
            {
                Guid currentUserId = User.GetLoggedInUserId();
                UserRoleEnum? currentRole = User.GetLoggedInUserRole();

                if (currentUserId == Guid.Empty || currentRole == null)
                    return Unauthorized();

                if (!await _ITenantAuthorizationService.CanAccessCompany(currentUserId, currentRole.Value, companyId))
                    return Forbid();

                List<ItemResponse> result = await _IItemsService.GetAllItems(companyId);
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet]
        [Route("get_item_by_id/{companyId}_{itemId}")]
        [Authorize(Roles = UserRolesHelper.SuperAdminRole + "," + UserRolesHelper.StoreAdminRole)]
        public async Task<IActionResult> GetItemById(Guid companyId, Guid itemId)
        {
            try
            {
                Guid currentUserId = User.GetLoggedInUserId();
                UserRoleEnum? currentRole = User.GetLoggedInUserRole();

                if (currentUserId == Guid.Empty || currentRole == null)
                    return Unauthorized();

                if (!await _ITenantAuthorizationService.CanAccessCompany(currentUserId, currentRole.Value, companyId))
                    return Forbid();

                ItemResponse? result = await _IItemsService.GetItemById(companyId, itemId);

                if (result == null)
                    return NotFound("No data present!");

                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet]
        [Route("get_item_by_name/{companyId}_{itemName}")]
        [Authorize(Roles = UserRolesHelper.SuperAdminRole + "," + UserRolesHelper.StoreAdminRole)]
        public async Task<IActionResult> GetItemByName(Guid companyId, string itemName)
        {
            try
            {
                Guid currentUserId = User.GetLoggedInUserId();
                UserRoleEnum? currentRole = User.GetLoggedInUserRole();

                if (currentUserId == Guid.Empty || currentRole == null)
                    return Unauthorized();

                if (!await _ITenantAuthorizationService.CanAccessCompany(currentUserId, currentRole.Value, companyId))
                    return Forbid();

                ItemResponse? result = await _IItemsService.GetItemByName(companyId, itemName);

                if (result == null)
                    return NotFound("No data present!");

                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost]
        [Route("add")]
        [Authorize(Roles = UserRolesHelper.SuperAdminRole + "," + UserRolesHelper.StoreAdminRole)]
        public async Task<IActionResult> AddItem([FromForm] ItemRequest newItem)
        {
            try
            {
                Guid currentUserId = User.GetLoggedInUserId();
                UserRoleEnum? currentRole = User.GetLoggedInUserRole();

                if (currentUserId == Guid.Empty || currentRole == null)
                    return Unauthorized();

                if (!await _ITenantAuthorizationService.CanAccessCompany(currentUserId, currentRole.Value, newItem.CompanyId))
                    return Forbid();

                ItemResponse? result = await _IItemsService.AddItem(newItem, currentUserId);

                if (result == null)
                    return BadRequest("Could not add item");

                return CreatedAtAction(nameof(AddItem), result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPut]
        [Route("update/{companyId}_{itemId}")]
        [Authorize(Roles = UserRolesHelper.SuperAdminRole + "," + UserRolesHelper.StoreAdminRole)]
        public async Task<IActionResult> UpdateItem(Guid companyId, Guid itemId, [FromForm] ItemRequest newItem)
        {
            try
            {
                Guid currentUserId = User.GetLoggedInUserId();
                UserRoleEnum? currentRole = User.GetLoggedInUserRole();

                if (currentUserId == Guid.Empty || currentRole == null)
                    return Unauthorized();

                if (!await _ITenantAuthorizationService.CanAccessCompany(currentUserId, currentRole.Value, companyId))
                    return Forbid();

                if (!await _IItemsService.UpdateItem(companyId, itemId, newItem))
                    return BadRequest("Could not update data!");

                return Ok("Data updated successfully!");
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpDelete]
        [Route("delete/{companyId}_{itemId}")]
        [Authorize(Roles = UserRolesHelper.SuperAdminRole + "," + UserRolesHelper.StoreAdminRole)]
        public async Task<IActionResult> DeleteItem(Guid companyId, Guid itemId)
        {
            try
            {
                Guid currentUserId = User.GetLoggedInUserId();
                UserRoleEnum? currentRole = User.GetLoggedInUserRole();

                if (currentUserId == Guid.Empty || currentRole == null)
                    return Unauthorized();

                if (!await _ITenantAuthorizationService.CanAccessCompany(currentUserId, currentRole.Value, companyId))
                    return Forbid();

                if (!await _IItemsService.DeleteItem(companyId, itemId))
                    return BadRequest("Could not delete data!");

                return Ok("Data deleted successfully!");
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPut]
        [Route("undelete/{companyId}_{itemId}")]
        [Authorize(Roles = UserRolesHelper.SuperAdminRole + "," + UserRolesHelper.StoreAdminRole)]
        public async Task<IActionResult> UnDeleteItem(Guid companyId, Guid itemId)
        {
            try
            {
                Guid currentUserId = User.GetLoggedInUserId();
                UserRoleEnum? currentRole = User.GetLoggedInUserRole();

                if (currentUserId == Guid.Empty || currentRole == null)
                    return Unauthorized();

                if (!await _ITenantAuthorizationService.CanAccessCompany(currentUserId, currentRole.Value, companyId))
                    return Forbid();

                if (!await _IItemsService.UnDeleteItem(companyId, itemId))
                    return BadRequest("Could not undelete data!");

                return Ok("Data undeleted successfully!");
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
