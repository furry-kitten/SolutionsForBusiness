using Infrastructure.DataBase.Models;
using Infrastructure.Repositories;
using PresentationLayer.Extensions;
using PresentationLayer.Utils;
using POrder = PresentationLayer.Models.Order;
using PProvider = PresentationLayer.Models.Provider;
using PItem = PresentationLayer.Models.OrderItem;

namespace PresentationLayer.Services
{
    public class ItemService : BaseService<PItem>
    {
        private readonly ItemRepository _itemRepository;
        private readonly OrderRepository _orderRepository;

        public ItemService(ItemRepository itemRepository, OrderRepository orderRepository)
        {
            _itemRepository = itemRepository;
            _orderRepository = orderRepository;
        }

        public override async Task<PItem> Save(PItem model)
        {
            if (string.IsNullOrWhiteSpace(model.Unit))
            {
                throw new ArgumentException($"{nameof(PItem.Unit)} cannot be empty");
            }

            if (model.Quantity <= 0)
            {
                throw new ArgumentException($"Please fill the {nameof(PItem.Quantity)}");
            }

            return await base.Save(model);
        }

        public override PItem[] GetByFilter(DataFilter<PItem> filter)
        {
            return _itemRepository.Get(item => filter.Filter(item.Convert()))
                                  .Select(Converter.Convert)
                                  .ToArray();
        }
        public override async Task Add(PItem model)
        {
            if (model.OrderId is null)
            {
                throw new ArgumentException("Item must by attached to the order");
            }

            var orderEntity = await _orderRepository.GetAsync(model.OrderId.Value);
            await _itemRepository.AddAsync(model.Convert(orderEntity));
        }

        public override async Task Update(PItem model)
        {
            await _itemRepository.UpdateAsync(model.Convert(null));
        }

        public override async Task Remove(PItem model)
        {
            await _itemRepository.Remove(new OrderItem
            {
                Id = model.Id
            });
        }

        public async Task RemoveRange(IEnumerable<PItem> items)
        {
            await _itemRepository.RemoveRangeAsync(items.Select(item => new OrderItem
            {
                Id = item.Id
            }));
        }
    }
}