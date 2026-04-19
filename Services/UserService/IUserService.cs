using Multi_Tenant_E_Commerce_API.Dtos.UserDtos;
using Multi_Tenant_E_Commerce_API.Helpers;

namespace Multi_Tenant_E_Commerce_API.Services.UserService
{
    public interface IUserService
    {
        public Task<List<UserResponse>> GetAllUsers(UserRoleEnum? currentRole, Guid currentUserId);
        public Task<List<UserResponse>> GetAllUsersByRole(UserRoleEnum? currentRole, Guid currentUserId, UserRoleEnum userRole);
        public Task<UserResponse?> GetUserByUserId(UserRoleEnum? currentRole, Guid currentUserId, Guid userId);
        public Task<UserResponse?> GetUserByName(UserRoleEnum? currentRole, Guid currentUserId, string fName, string lName);
        public Task<UserResponse?> GetUserByEmail(UserRoleEnum? currentRole, Guid currentUserId, string userEmail);

        public Task<UserResponse?> TestAddUser(UserRequest userRequest);
        public Task<UserResponse?> AddUser(UserRequest userRequest, UserRoleEnum? currentRole, Guid? currentUserId);
        public Task<bool> UpdateUser(Guid userId, UserRequest userRequest, UserRoleEnum? currentRole, Guid? currentUserId);

        public Task<bool> DeleteUser(Guid userId, UserRoleEnum? currentRole, Guid? currentUserId);
        public Task<bool> UnDeleteUser(Guid userId);
    }
}
