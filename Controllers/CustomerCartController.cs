using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multi_Tenant_E_Commerce_API.Dtos.CartDtos;
using Multi_Tenant_E_Commerce_API.Dtos.OrderDtos;
using Multi_Tenant_E_Commerce_API.Helpers;
using Multi_Tenant_E_Commerce_API.Services.CustomerService;

namespace Multi_Tenant_E_Commerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerCartController : ControllerBase
    {
        private ICustomerCartService _ICustomerCartService;

        public CustomerCartController(ICustomerCartService customerCartService)
        {
            _ICustomerCartService = customerCartService;
        }

        [HttpGet]
        [Route("get_cart_details")]
        [Authorize(Roles = UserRolesHelper.SuperAdminRole + "," + UserRolesHelper.StoreAdminRole + "," + UserRolesHelper.CustomerRole)]
        public async Task<IActionResult> GetCartDetails(Guid userId)
        {
            try
            {
                Guid userID = User.CheckCurrentRole(UserRoleEnum.Customer) ? User.GetLoggedInUserId() : userId;

                CartResponse? result = await _ICustomerCartService.GetCartDetails(userID);

                if (result == null)
                    return NotFound("No data present!");

                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost]
        [Route("add_item")]
        [Authorize(Roles = UserRolesHelper.CustomerRole)]
        public async Task<IActionResult> AddItemToCart([FromBody] CartItemRequest newCartItem)
        {
            try
            {
                Guid userID = User.GetLoggedInUserId();

                if (userID == Guid.Empty)
                    return Unauthorized();

                if (newCartItem.Quantity <= 0)
                    return BadRequest("Quantity must be greater than zero");

                (string, CartResponse?) result = await _ICustomerCartService.AddItemToCart(userID, newCartItem);

                if (result.Item2 == null)
                    return BadRequest(result.Item1);

                return Ok(result.Item2);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPut]
        [Route("update_item")]
        [Authorize(Roles = UserRolesHelper.CustomerRole)]
        public async Task<IActionResult> UpdateCartItem([FromBody] CartItemRequest newCartItem)
        {
            try
            {
                Guid userID = User.GetLoggedInUserId();

                if (userID == Guid.Empty)
                    return Unauthorized();

                if (newCartItem.Quantity <= 0)
                    return BadRequest("Quantity must be greater than zero");

                (string, CartResponse?) result = await _ICustomerCartService.UpdateCartItem(userID, newCartItem);

                if (result.Item2 == null)
                    return BadRequest(result.Item1);

                return Ok(result.Item2);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpDelete]
        [Route("remove_item/{cartItemId}")]
        [Authorize(Roles = UserRolesHelper.CustomerRole)]
        public async Task<IActionResult> RemoveCartItem(Guid cartItemId)
        {
            try
            {
                Guid userID = User.GetLoggedInUserId();

                if (userID == Guid.Empty)
                    return Unauthorized();

                (string, CartResponse?) result = await _ICustomerCartService.RemoveCartItem(userID, cartItemId);

                if (result.Item2 == null)
                    return BadRequest(result.Item1);

                return Ok(result.Item2);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpDelete]
        [Route("remove_cart")]
        [Authorize(Roles = UserRolesHelper.SuperAdminRole + "," + UserRolesHelper.StoreAdminRole + "," + UserRolesHelper.CustomerRole)]
        public async Task<IActionResult> RemoveCart(Guid userId)
        {
            try
            {
                Guid userID = User.CheckCurrentRole(UserRoleEnum.Customer) ? User.GetLoggedInUserId() : userId;

                if (!await _ICustomerCartService.RemoveCart(userID))
                    return BadRequest("Could not remove cart!");

                return Ok("Removed cart successfuly");
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet]
        [Route("checkout")]
        [Authorize(Roles = UserRolesHelper.CustomerRole)]
        public async Task<IActionResult> CheckOutCart()
        {
            try
            {
                Guid userID = User.GetLoggedInUserId();

                if (userID == Guid.Empty)
                    return Unauthorized();

                OrderResponse? result = await _ICustomerCartService.CheckOutCart(userID);

                if (result == null)
                    return BadRequest("could not checkout!");

                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
