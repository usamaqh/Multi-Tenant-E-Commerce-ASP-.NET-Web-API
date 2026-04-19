using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Multi_Tenant_E_Commerce_API.Models
{
    public class Cart
    {
        [Required] public int Id { get; set; } // primary key
        [Required] public Guid CartId { get; set; } // search key
        [Required] public decimal TotalPrice { get; set; } = 0;
        [Required] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

        #region FKs
        [Required] public int UserId { get; set; }
        [ForeignKey(nameof(UserId))] public User Customer { get; set; }
        #endregion FKs
    }
}
