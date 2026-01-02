namespace Store.Core.Entities
{
    public class ProductReview : BaseEntity
    {
        public int Rating { get; set; } // 1-5
        public string Comment { get; set; } = string.Empty;
        
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = null!;
    }
}
