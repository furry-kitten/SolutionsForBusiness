using Infrastructure.DataBase;
using Infrastructure.DataBase.Models;
using Infrastructure.Extensions;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ItemRepository : BaseRepository<Item>, IItemRepository
    {
        public ItemRepository(EfCoreContext context) : base(context) { }
        public IEnumerable<Item> GetFullInfo<TType>(DataFilter<Item, TType> filter)
        {
            return WriteEntity.Include(item => item.Order).AsNoTracking().AsEnumerable().Where(filter.Filter);
        }

        public override Item Add(Item entity)
        {
            Context.Add(entity);
            Context.SaveChanges();
            Context.Detach(entity);

            return entity;
        }
    }
}