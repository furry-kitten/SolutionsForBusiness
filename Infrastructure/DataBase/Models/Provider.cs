namespace Infrastructure.DataBase.Models
{
    public class Provider : BaseDbModel
    {
        public string? Name { get; set; }

        public ICollection<Order>? Orders { get; set; }
    }
}
