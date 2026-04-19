using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Multi_Tenant_E_Commerce_API.Models
{
    public class OrderItem
    {
        [Required] public int Id { get; set; } // primary key
        [Required] public string ItemName { get; set; }
        public string? ItemImageURL { get; set; }
        [Required] public decimal ItemPrice { get; set; }
        [Required] public int Quantity { get; set; }

        #region FKs
        [Required] public int OrderId { get; set; }
        [ForeignKey(nameof(OrderId))] public Order Order { get; set; }
        #endregion FKs
    }
}
