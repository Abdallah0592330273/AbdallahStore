using Store.Core.Entities;

namespace Store.ModelView
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public AddressDto ShipToAddress { get; set; } = null!;
        public string DeliveryMethod { get; set; } = string.Empty;
        public decimal ShippingPrice { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new();
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string PictureUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
