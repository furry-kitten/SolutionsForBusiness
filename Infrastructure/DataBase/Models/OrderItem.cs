namespace Infrastructure.DataBase.Models
{
    public class OrderItem : BaseDbModel
    {
        public string? Name { get; set; }
        public decimal? Quantity { get; set; }
        public string? Unit { get; set; }
        public int? OrderId { get; set; }

        public Order? Order { get; set; }
    }
}
