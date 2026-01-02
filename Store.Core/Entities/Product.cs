namespace Store.Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string PictureUrl { get; set; } = string.Empty;
        public string? PicturePublicId { get; set; }
        
        public ProductType ProductType { get; set; } = null!;
        public int ProductTypeId { get; set; }
        
        public ProductBrand ProductBrand { get; set; } = null!;
        public int ProductBrandId { get; set; }

        public Category Category { get; set; } = null!;
        public int CategoryId { get; set; }

        public List<ProductImage> ProductImages { get; set; } = new();
        public List<ProductReview> ProductReviews { get; set; } = new();
    }
}
