using Infrastructure.DataBase.Models;
using Infrastructure.Repositories;
using Infrastructure.Utils;
using PresentationLayer.Extensions;
using POrder = PresentationLayer.Models.Order;
using PProvider = PresentationLayer.Models.Provider;
using PItem = PresentationLayer.Models.Item;

namespace PresentationLayer.Services
{
    public class ItemService : BaseService<Item, PItem>, IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IOrderRepository _orderRepository;

        public ItemService(IItemRepository itemRepository, IOrderRepository orderRepository)
        {
            _itemRepository = itemRepository;
            _orderRepository = orderRepository;
        }

        public override PItem Save(PItem model)
        {
            if (string.IsNullOrWhiteSpace(model.Unit))
            {
                throw new ArgumentException($"{nameof(PItem.Unit)} cannot be empty");
            }

            if (model.Quantity <= 0)
            {
                throw new ArgumentException($"Please fill the {nameof(PItem.Quantity)}");
            }

            return base.Save(model);
        }

        public override List<PItem>
            GetByFilter<TEntityType>(DataFilter<Item, TEntityType> filter) =>
            _itemRepository.Get(filter).Convert().ToList();
        public override void Add(PItem model)
        {
            if (model.OrderId is null)
            {
                throw new ArgumentException("Item must by attached to the order");
            }

            if (model.Name!.Equals(model.OrderNumber))
            {
                throw new ArgumentException("Item name cannot be equals order number");
            }

            var orderEntity = _orderRepository.GetFullInfo(model.OrderId.Value);
            var convert = model.Convert(orderEntity);
            _itemRepository.Add(convert);
        }

        public override void Update(PItem model)
        {
            var entity = model.Convert(null);
            _itemRepository.Update(entity);
        }

        public override void Remove(PItem model)
        {
            _itemRepository.Remove(new Item
            {
                Id = model.Id
            });
        }

        public override PItem? Get(int id) => _itemRepository.Get(id)?.Convert();

        public override List<PItem>
            GetFullByFilter<TEntityType>(DataFilter<Item, TEntityType> filter) =>
            _itemRepository.GetFullInfo(filter).Convert().ToList();

        public async Task RemoveRange(IEnumerable<PItem> items)
        {
            await _itemRepository.RemoveRangeAsync(items.Select(item => new Item
            {
                Id = item.Id
            }));
        }
    }
}