using Infrastructure.Utils;
using PresentationLayer.Models;

namespace PresentationLayer.Services
{
    public interface IOrderService : IService<Infrastructure.DataBase.Models.Order, Order>
    {
        Order? GetByNumberAndProvider(string number, int providerId);
        List<Order> GetFullByFilter<TEntityType>(DataFilter<Infrastructure.DataBase.Models.Order, TEntityType> filter);
        List<Order> GetFullByFilter(Func<Infrastructure.DataBase.Models.Order, bool> predicate);
        Order Create(string? number, string provider, IEnumerable<Item> items);
        IEnumerable<Order> Get();
        Order? GetFull(int id);
    }
}