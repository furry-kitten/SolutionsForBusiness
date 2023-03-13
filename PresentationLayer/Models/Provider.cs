namespace PresentationLayer.Models
{
    public class Provider : BaseModel
    {
        public string? Name { get; set; }
        public IEnumerable<Order>? Orders { get; set; }
    }
}
