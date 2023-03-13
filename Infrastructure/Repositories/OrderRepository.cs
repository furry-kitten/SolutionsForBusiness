using System.Linq.Expressions;
using Infrastructure.DataBase;
using Infrastructure.DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class OrderRepository : BaseRepository<Order>
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

            return base.Add(entity);
        }

        public async Task<Order?> GetFullInfo(int id) =>
            await WriteEntity.Include(order => order.Provider)
                         .Include(order => order.Items)
                         .FirstOrDefaultAsync(order => order.Id == id);

        public IEnumerable<Order?> GetFullInfo(Expression<Func<Order, bool>> predicate) =>
            WriteEntity.Include(order => order.Provider)
                         .Include(order => order.Items).Where(predicate);

        public async Task<Order?> GetByNumberAndProvider(string number, int providerId)
        {
            return await WriteEntity.Include(order => order.Provider)
                                    .Include(order => order.Items)
                                    .Where(order =>
                                        number.Equals(order.Number) &&
                                        order.ProviderId == providerId)
                                    .FirstOrDefaultAsync();
        }

        public IEnumerable<Order> GetByNumberAndProvider(Expression<Func<Order, bool>> predicate) =>
            WriteEntity.Include(order => order.Provider).Where(predicate);
    }
}