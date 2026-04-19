using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Multi_Tenant_E_Commerce_API.Models
{
    public class CartItem
    {
        [Required] public int Id { get; set; } // primary key
        [Required] public int Quantity { get; set; }

        #region FKs
        [Required] public int CartId { get; set; }
        [ForeignKey(nameof(CartId))] public Cart Cart { get; set; }

        [Required] public int ItemId { get; set; }
        [ForeignKey(nameof(ItemId))] public Item Item { get; set; }
        #endregion FKs
    }
}
