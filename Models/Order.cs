using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Multi_Tenant_E_Commerce_API.Models
{
    public class Order
    {
        [Required] public int Id { get; set; } // primary key
        [Required] public Guid OrderId { get; set; } // search key
        [Required] public decimal TotalPrice { get; set; } = 0;
        [Required] public DateTime PaidAt { get; set; } = DateTime.UtcNow;
        [Required] public bool IsDeleted { get; set; } = false;

        #region FKs
        [Required] public int CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))] public User Customer { get; set; }
        #endregion FKs
    }
}