namespace PresentationLayer.Models
{
    public class Order : BaseModel
    {
        public string? Number { get; set; }
        public DateTime? Date { get; set; }
        public string? Provider { get; set; }
        public IEnumerable<OrderItem>? Items { get; set; }
    }
}
