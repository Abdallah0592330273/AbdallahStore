namespace Store.ModelView
{
    public class ProductCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string PictureUrl { get; set; } = string.Empty;
        public int ProductTypeId { get; set; }
        public int ProductBrandId { get; set; }
        public int CategoryId { get; set; }
    }
}
