using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Multi_Tenant_E_Commerce_API.Models
{
    public class Item
    {
        [Required] public int Id { get; set; } // primary key
        [Required] public Guid ItemId { get; set; } // search key
        [Required] public string Name { get; set; }
        public string? ImagePath { get; set; }
        [Required] public int InStock { get; set; }
        [Required] public decimal Price { get; set; }
        public int SoldUnits { get; set; } = 0;
        public decimal Revenue { get; set; } = 0;
        [Required] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required] public bool IsDeleted { get; set; } = false;

        #region FKs
        [Required] public int CompanyId { get; set; } // Item of which company
        [ForeignKey(nameof(CompanyId))] public Company Company { get; set; }

        public int? UserId { get; set; } // Item added by which store admin
        [ForeignKey(nameof(UserId))] public User? AddedByUser { get; set; }
        #endregion FKs
    }
}
