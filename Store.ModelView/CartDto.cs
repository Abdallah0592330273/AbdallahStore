namespace Store.ModelView
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string PictureUrl { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }

    public class CartDto
    {
        public int Id { get; set; }
        public string BuyerId { get; set; } = string.Empty;
        public List<CartItemDto> Items { get; set; } = new();
    }
}
