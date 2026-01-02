namespace Store.Core.Entities
{
    public class ProductImage : BaseEntity
    {
        public string PictureUrl { get; set; } = string.Empty;
        public string? PicturePublicId { get; set; }
        public bool IsMain { get; set; }
        
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
