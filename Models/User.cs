using Multi_Tenant_E_Commerce_API.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Multi_Tenant_E_Commerce_API.Models
{
    public class User
    {
        [Required] public int Id { get; set; } // primary key
        [Required] public Guid UserId { get; set; } // search key
        [Required] public UserRoleEnum Role { get; set; }
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public string Email { get; set; }
        [Required] public string PasswordHash { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ImagePath { get; set; }
        [Required] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required] public bool IsDeleted { get; set; } = false;

        public Guid CompanyId { get; set; }
        public ICollection<Company>? Companies { get; set; }

        #region FKs
        #region ForStoreAdmin
        public int? CreatedByUserId { get; set; }
        [ForeignKey(nameof(CreatedByUserId))] public User? CreatedByUser { get; set; }
        #endregion ForStoreAdmin
        #endregion FKs
    }
}
