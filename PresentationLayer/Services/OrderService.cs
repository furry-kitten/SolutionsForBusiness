using Infrastructure.DataBase.Models;
using Infrastructure.Repositories;
using Infrastructure.Utils;
using PresentationLayer.Enums;
using PresentationLayer.Extensions;
using POrder = PresentationLayer.Models.Order;
using PProvider = PresentationLayer.Models.Provider;
using PItem = PresentationLayer.Models.Item;

namespace PresentationLayer.Services
{
    public class OrderService : BaseService<Order, POrder>, IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProviderRepository _providerRepository;
        private readonly IItemRepository _itemRepository;

        public OrderService(IOrderRepository orderRepository,
            IProviderRepository providerRepository,
            IItemRepository itemRepository)
        {
            _orderRepository = orderRepository;
            _providerRepository = providerRepository;
            _itemRepository = itemRepository;
        }

        public override POrder Save(POrder model)
        {
            if (model.State is ModelState.Detached)
            {
                return base.Save(model);
            }

            var provider = _providerRepository.GetByName(model.Provider);
            if (provider == null)
            {
                throw new ArgumentException($"There is no provider with name {model.Provider}");
            }

            if (model.Items?.Any(item => item.Name.Equals(model.Number)) == true)
            {
                throw new ArgumentException("Order number and order item name must be different");
            }

            if (model.State is ModelState.Added &&
                _orderRepository.GetByNumberAndProvider(model.Number, provider.Id) != null)
            {
                throw new ArgumentException("Order duplication");
            }

            return base.Save(model);
        }

        public override List<POrder> GetFullByFilter<TEntityType>(
            DataFilter<Order, TEntityType> filter) =>
            _orderRepository.GetFullInfo(filter).Convert().ToList();

        public List<POrder> GetFullByFilter(Func<Order, bool> predicate) =>
            _orderRepository.GetFullInfo(predicate).Convert().ToList();

        public POrder? GetByNumberAndProvider(string number, int providerId) =>
            _orderRepository.GetByNumberAndProvider(number, providerId)?.Convert();

        public POrder? GetFull(int id) => _orderRepository.GetFullInfo(id)?.Convert();

        public override List<POrder>
            GetByFilter<TEntityType>(DataFilter<Order, TEntityType> filter) =>
            _orderRepository.GetByFilter(filter).Convert().ToList();

        public override void Add(POrder model)
        {
            var provider = _providerRepository.GetByName(model.Provider);
            provider ??= new Provider
            {
                Name = model.Provider
            };

            var entity = model.Convert(provider);
            entity.Provider = provider.Id > 0 ? null : provider;
            _orderRepository.Add(entity);
        }

        public override void Update(POrder model)
        {
            var provider = _providerRepository.GetByName(model.Provider);
            var entity = model.Convert(provider);
            _orderRepository.Update(entity);
        }

        public override void Remove(POrder pOrder)
        {
            var items = _itemRepository.Get(item => item.OrderId == pOrder.Id);
            _itemRepository.RemoveRange(items);
            _orderRepository.Remove(new Order
            {
                Id = pOrder.Id
            });
        }

        public POrder Create(string? number, string provider, IEnumerable<PItem> items)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentNullException(nameof(number));
            }

            var order = GetDefaultModel();
            order.Number = number;
            order.Items = items;
            order.Provider = provider;

            return Save(order);
        }

        public override POrder? Get(int id) => _orderRepository.Get(id)?.Convert();

        public IEnumerable<POrder> Get() => _orderRepository.Get().Convert().ToList();

        public override POrder GetDefaultModel()
        {
            var model = base.GetDefaultModel();
            model.Date = DateTime.UtcNow;
            return model;
        }
    }
}