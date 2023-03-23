using Infrastructure.DataBase.Models;
using Infrastructure.Utils;

namespace Infrastructure.Repositories
{
    public interface IOrderRepository
    {
        Order Add(Order entity);
        Order? GetFullInfo(int id);
        IEnumerable<Order> GetFullInfo<TType>(DataFilter<Order, TType> filter);
        IEnumerable<Order> GetFullInfo(Func<Order, bool> predicate);
        Order? GetByNumberAndProvider(string number, int providerId);
        IEnumerable<Order> GetByFilter<TType>(DataFilter<Order, TType> filter);
        Order? Get(int id);
        IEnumerable<Order> Get();
        IEnumerable<Order> Get<TType>(DataFilter<Order, TType> filter);
        Task<Order> AddAsync(Order entity);
        void UpdateRange(IEnumerable<Order> entities);
        void Update(Order entity);
        Task UpdateAsync(Order entity);
        void AddRange(IEnumerable<Order> entities);
        Task<int> Remove(Order entity);
        int RemoveRange(IEnumerable<Order> entities);
        Task<int> RemoveRangeAsync(IEnumerable<Order> entities);
    }
}