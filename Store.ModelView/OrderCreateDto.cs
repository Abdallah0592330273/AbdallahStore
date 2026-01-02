namespace Store.ModelView
{
    public class OrderCreateDto
    {
        public int DeliveryMethodId { get; set; }
        public string CartId { get; set; } = string.Empty;
        public AddressDto ShippingAddress { get; set; } = null!;
    }
}
// Moving this to Store.Application.Dtos for consistency
