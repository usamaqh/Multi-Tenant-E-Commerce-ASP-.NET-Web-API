using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Multi_Tenant_E_Commerce_API.Models
{
    public class Company
    {
        [Required] public int Id { get; set; } // primary key
        [Required] public Guid CompanyId { get; set; } // search key
        [Required] public string Name { get; set; }
        [Required] public string Email { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Website { get; set; }
        public string? ImagePath { get; set; }
        [Required] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required] public bool IsDeleted { get; set; } = false;

        #region FKs
        public int? AdminId { get; set; }
        [ForeignKey(nameof(AdminId))] public User? CreatedBy { get; set; }
        #endregion FKs
    }
}
