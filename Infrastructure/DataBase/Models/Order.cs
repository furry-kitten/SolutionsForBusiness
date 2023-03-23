namespace Infrastructure.DataBase.Models
{
    public class Order : BaseDbModel
    {
        public string? Number { get; set; }
        public DateTime? Date { get; set; }
        public int? ProviderId { get; set; }

        public Provider? Provider { get; set; }
        public ICollection<Item>? Items { get; set; }
    }
}
