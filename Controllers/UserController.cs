using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multi_Tenant_E_Commerce_API.Dtos.UserDtos;
using Multi_Tenant_E_Commerce_API.Helpers;
using Multi_Tenant_E_Commerce_API.Services.Authorization;
using Multi_Tenant_E_Commerce_API.Services.UserService;

namespace Multi_Tenant_E_Commerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _IUserService;
        private ITenantAuthorizationService _ITenantAuthorizationService;

        public UserController(IUserService iSuperAdmin, ITenantAuthorizationService tenantAuthorizationService)
        {
            _IUserService = iSuperAdmin;
            _ITenantAuthorizationService = tenantAuthorizationService;
        }

        [HttpGet]
        [Route("get_all")]
        [Authorize(Roles = UserRolesHelper.SuperAdminRole + "," + UserRolesHelper.StoreAdminRole)]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                UserRoleEnum? currentRole = User.GetLoggedInUserRole();
                Guid currentUserId = User.GetLoggedInUserId();

                if (currentRole == null || currentUserId == Guid.Empty)
                    return Unauthorized();

                List<UserResponse> result = await _IUserService.GetAllUsers(currentRole, currentUserId);

                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet]
        [Route("get_all_by_roll/{userRoll}")]
        [Authorize(Roles = UserRolesHelper.SuperAdminRole + "," + UserRolesHelper.StoreAdminRole)]
        public async Task<IActionResult> GetAllUsersByRole(UserRoleEnum userRoll)
        {
            try
            {
                UserRoleEnum? currentRole = User.GetLoggedInUserRole();
                Guid currentUserId = User.GetLoggedInUserId();

                if (currentRole == null || currentUserId == Guid.Empty)
                    return Unauthorized();

                List<UserResponse> result = await _IUserService.GetAllUsersByRole(currentRole, currentUserId, userRoll);

                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet]
        [Route("get_by_userid/{userId}")]
        [Authorize(Roles = UserRolesHelper.SuperAdminRole + "," + UserRolesHelper.StoreAdminRole)]
        public async Task<IActionResult> GetUserByUserId(Guid userId)
        {
            try
            {
                UserRoleEnum? currentRole = User.GetLoggedInUserRole();
                Guid currentUserId = User.GetLoggedInUserId();

                if (currentRole == null || currentUserId == Guid.Empty)
                    return Unauthorized();

                UserResponse? result = await _IUserService.GetUserByUserId(currentRole, currentUserId, userId);

                if (result == null)
                    return NotFound("No data present!");

                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet]
        [Route("get_by_name/{fname}_{lname}")]
        [Authorize(Roles = UserRolesHelper.SuperAdminRole + "," + UserRolesHelper.StoreAdminRole)]
        public async Task<IActionResult> GetUserByName(string fname, string lname)
        {
            try
            {
                UserRoleEnum? currentRole = User.GetLoggedInUserRole();
                Guid currentUserId = User.GetLoggedInUserId();

                if (currentRole == null || currentUserId == Guid.Empty)
                    return Unauthorized();

                UserResponse? result = await _IUserService.GetUserByName(currentRole, currentUserId, fname, lname);

                if (result == null)
                    return NotFound("No data present!");

                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet]
        [Route("get_by_email/{email}")]
        [Authorize(Roles = UserRolesHelper.SuperAdminRole + "," + UserRolesHelper.StoreAdminRole + "," + UserRolesHelper.CustomerRole)]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            try
            {
                UserRoleEnum? currentRole = User.GetLoggedInUserRole();
                Guid currentUserId = User.GetLoggedInUserId();

                if (currentRole == null || currentUserId == Guid.Empty)
                    return Unauthorized();

                UserResponse? result = await _IUserService.GetUserByEmail(currentRole, currentUserId, email);

                if (result == null)
                    return NotFound("No data present!");

                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        #region TestAdd
        //[HttpPost]
        //[Route("test_add")]
        //public async Task<IActionResult> TestAddUser([FromForm] UserRequest newUserRequest)
        //{
        //    try
        //    {
        //        UserResponse? result = await _IUserService.TestAddUser(newUserRequest);

        //        if (result == null)
        //            return BadRequest("Failed adding data!");

        //        return CreatedAtAction(nameof(AddUser), result);
        //    }
        //    catch (Exception ex) { return BadRequest(ex.Message); }
        //}
        #endregion TestAdd

        [HttpPost]
        [Route("add")]
        [Authorize(Roles = UserRolesHelper.SuperAdminRole + "," + UserRolesHelper.StoreAdminRole)]
        public async Task<IActionResult> AddUser([FromForm] UserRequest newUserRequest)
        {
            try
            {
                UserRoleEnum? currentRole = User.GetLoggedInUserRole();
                Guid currentUserId = User.GetLoggedInUserId();

                if (currentRole == null || currentUserId == Guid.Empty)
                    return Unauthorized();

                if (currentRole == UserRoleEnum.StoreAdmin && newUserRequest.Role == UserRoleEnum.SuperAdmin)
                    return Unauthorized();

                UserResponse? result = await _IUserService.AddUser(newUserRequest, currentRole, currentUserId);

                if (result == null)
                    return BadRequest("Failed adding data!");

                return CreatedAtAction(nameof(AddUser), result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPut]
        [Route("update/{userId}")]
        [Authorize(Roles = UserRolesHelper.SuperAdminRole + "," + UserRolesHelper.StoreAdminRole + "," + UserRolesHelper.CustomerRole)]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromForm] UserRequest newUserRequest)
        {
            try
            {
                UserRoleEnum? currentRole = User.GetLoggedInUserRole();
                Guid currentUserId = User.GetLoggedInUserId();

                if (currentRole == null || currentUserId == Guid.Empty)
                    return Unauthorized();

                if (currentRole == UserRoleEnum.StoreAdmin && newUserRequest.Role == UserRoleEnum.SuperAdmin)
                    return Unauthorized();

                if (!await _IUserService.UpdateUser(userId, newUserRequest, currentRole, currentUserId))
                    return BadRequest("Could not update data!");

                return Ok("Data updated successfully!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("delete/{userId}")]
        [Authorize(Roles = UserRolesHelper.SuperAdminRole + "," + UserRolesHelper.StoreAdminRole + "," + UserRolesHelper.CustomerRole)]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            try
            {
                UserRoleEnum? currentRole = User.GetLoggedInUserRole();
                Guid currentUserId = User.GetLoggedInUserId();

                if (currentRole == null || currentUserId == Guid.Empty)
                    return Unauthorized();

                if (currentRole == UserRoleEnum.Customer && currentUserId != userId)
                    return Forbid();

                if (!await _IUserService.DeleteUser(userId, currentRole, currentUserId))
                    return BadRequest("Could not delete data!");

                return Ok("Data deleted successfully!");
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPut]
        [Route("undelete/{userId}")]
        [Authorize(Roles = UserRolesHelper.SuperAdminRole)]
        public async Task<IActionResult> UnDeleteUser(Guid userId)
        {
            try
            {
                if (!await _IUserService.UnDeleteUser(userId))
                    return BadRequest("Could not undelete data!");

                return Ok("Data undeleted successfuly!");
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
