using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Core.Entities
{
    public class Wishlist : BaseEntity
    {
        public int ApplicationUserId { get; set; }
        
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; } = null!;
        
        public List<WishlistItem> Items { get; set; } = new();
    }

    public class WishlistItem : BaseEntity
    {
        public int WishlistId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
