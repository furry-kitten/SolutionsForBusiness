namespace PresentationLayer.Models
{
    public class Item : BaseModel
    {
        public string? Name { get; set; }
        public decimal? Quantity { get; set; }
        public string? Unit { get; set; }
        public string? OrderNumber { get; set; }
        public int? OrderId { get; set; }
    }
}
