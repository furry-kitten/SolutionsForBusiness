using System.Linq.Expressions;
using Infrastructure.DataBase.Models;
using Infrastructure.Utils;

namespace Infrastructure.Repositories
{
    public interface IItemRepository
    {
        IEnumerable<Item> GetFullInfo<TType>(DataFilter<Item, TType> filter);
        Item? Get(int id);
        IEnumerable<Item> Get();
        IEnumerable<Item> Get<TType>(DataFilter<Item, TType> filter);
        IEnumerable<Item> Get(Expression<Func<Item, bool>> predicate);
        Item Add(Item entity);
        Task<Item> AddAsync(Item entity);
        void UpdateRange(IEnumerable<Item> entities);
        void Update(Item entity);
        Task UpdateAsync(Item entity);
        void AddRange(IEnumerable<Item> entities);
        int Remove(Item entity);
        int RemoveRange(IEnumerable<Item> entities);
        Task<int> RemoveRangeAsync(IEnumerable<Item> entities);
    }
}