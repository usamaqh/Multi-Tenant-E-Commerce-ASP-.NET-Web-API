namespace Multi_Tenant_E_Commerce_API.Helpers
{
    public static class UserRolesHelper
    {
        public const string SuperAdminRole = nameof(UserRoleEnum.SuperAdmin);
        public const string StoreAdminRole = nameof(UserRoleEnum.StoreAdmin);
        public const string CustomerRole = nameof(UserRoleEnum.Customer);
    }

    public enum UserRoleEnum
    {
        SuperAdmin,
        StoreAdmin,
        Customer
    }
}
