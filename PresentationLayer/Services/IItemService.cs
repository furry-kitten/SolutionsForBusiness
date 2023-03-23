using Infrastructure.Utils;
using PresentationLayer.Models;

namespace PresentationLayer.Services
{
    public interface IItemService
    {
        Item Save(Item model);
        List<Item> GetByFilter<TEntityType>(DataFilter<Infrastructure.DataBase.Models.Item, TEntityType> filter);
        void Add(Item model);
        void Update(Item model);
        void Remove(Item model);
        Item? Get(int id);
        List<Item>
            GetFullByFilter<TEntityType>(DataFilter<Infrastructure.DataBase.Models.Item, TEntityType> filter);
        Task RemoveRange(IEnumerable<Item> items);
        Item GetDefaultModel();
        void Attach(Item model);
        void Detached(Item model);
    }
}