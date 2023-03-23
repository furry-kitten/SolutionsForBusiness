using System.Linq.Expressions;
using Infrastructure.DataBase.Models;
using Infrastructure.Utils;

namespace Infrastructure.Repositories
{
    public interface IProviderRepository
    {
        Provider? GetByName(string? name);
        Provider? Get(int id);
        IEnumerable<Provider> Get();
        IEnumerable<Provider> Get<TType>(DataFilter<Provider, TType> filter);
        IEnumerable<Provider> Get<TType>(Expression<Func<Provider, bool>> predicate);
        Provider Add(Provider entity);
        Task<Provider> AddAsync(Provider entity);
        void UpdateRange(IEnumerable<Provider> entities);
        void Update(Provider entity);
        Task UpdateAsync(Provider entity);
        void AddRange(IEnumerable<Provider> entities);
        Task<int> Remove(Provider entity);
        int RemoveRange(IEnumerable<Provider> entities);
        Task<int> RemoveRangeAsync(IEnumerable<Provider> entities);
    }
}