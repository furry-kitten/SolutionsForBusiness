namespace PresentationLayer.Models
{
    public class OrderItem : BaseModel
    {
        public string? Name { get; set; }
        public decimal? Quantity { get; set; }
        public string? Unit { get; set; }
        public string? Order { get; set; }
        public int? OrderId { get; set; }
    }
}
