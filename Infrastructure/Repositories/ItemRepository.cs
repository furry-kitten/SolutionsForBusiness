using Infrastructure.DataBase;
using Infrastructure.DataBase.Models;

namespace Infrastructure.Repositories
{
    public class ItemRepository : BaseRepository<OrderItem>
    {
        public ItemRepository(EfCoreContext context) : base(context) { }
    }
}