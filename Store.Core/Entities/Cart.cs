namespace Store.Core.Entities
{
    public class Cart : BaseEntity
    {
        public string BuyerId { get; set; } = string.Empty;
        public List<CartItem> Items { get; set; } = new();
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
    }

    public class CartItem : BaseEntity
    {
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int CartId { get; set; }
    }
}
