using Infrastructure.DataBase;
using Infrastructure.DataBase.Models;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(EfCoreContext context) : base(context) { }

        public override Order Add(Order entity)
        {
            if (WriteEntity.Any(order =>
                    string.IsNullOrWhiteSpace(entity.Number) == false &&
                    entity.Number.Equals(order.Number) &&
                    entity.ProviderId == order.ProviderId))
            {
                throw new ArgumentException("This order is already exists");
            }

            return base.Add(new Order
            {
                Provider = entity.Provider,
                Number = entity.Number,
                Date = entity.Date,
                ProviderId = entity.ProviderId,
                Items = entity.Items
            });
        }

        public Order? GetFullInfo(int id) =>
            WriteEntity.Include(order => order.Provider)
                       .Include(order => order.Items)
                       .AsNoTracking()
                       .FirstOrDefault(order => order.Id == id);

        public override void Update(Order entity)
        {
            foreach (var item in entity.Items)
            {
                Context.Attach(item);
            }

            base.Update(entity);
        }

        public IEnumerable<Order> GetFullInfo<TType>(DataFilter<Order, TType> filter) =>
            WriteEntity.Include(order => order.Provider)
                       .Include(order => order.Items)
                       .AsNoTracking()
                       .AsEnumerable()
                       .Where(filter.Filter);

        public Order? GetByNumberAndProvider(string number, int providerId)
        {
            return WriteEntity.Include(order => order.Provider)
                              .Include(order => order.Items)
                              .AsNoTracking()
                              .FirstOrDefault(order =>
                                  number.Equals(order.Number) && order.ProviderId == providerId);
        }

        public IEnumerable<Order> GetByFilter<TType>(DataFilter<Order, TType> filter) =>
            WriteEntity.Include(order => order.Provider)
                       .AsNoTracking()
                       .AsEnumerable()
                       .Where(filter.Filter);

        public IEnumerable<Order> GetFullInfo(Func<Order, bool> predicate) =>
            WriteEntity.Include(order => order.Provider)
                       .Include(order => order.Items)
                       .AsNoTracking()
                       .AsEnumerable()
                       .Where(predicate);
    }
}