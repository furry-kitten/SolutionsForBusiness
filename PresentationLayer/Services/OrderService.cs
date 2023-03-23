using Infrastructure.DataBase.Models;
using Infrastructure.Repositories;
using Infrastructure.Utils;
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

        public OrderService(IOrderRepository orderRepository, IProviderRepository providerRepository)
        {
            _orderRepository = orderRepository;
            _providerRepository = providerRepository;
        }

        public override POrder Save(POrder model)
        {
            var provider = _providerRepository.GetByName(model.Provider);
            //if (provider == null)
            //{
            //    throw new ArgumentException($"There is no provider with name {model.Provider}");
            //}

            //if (!model.Items?.Any() != true)
            //{
            //    throw new ArgumentException("Order must contain at least one item");
            //}

            return base.Save(model);
        }

        public override List<POrder> GetFullByFilter<TEntityType>(
            DataFilter<Order, TEntityType> filter) =>
            _orderRepository.GetFullInfo(filter)
                            //.Select(order =>
                            //{
                            //    order.Provider = new Provider(){Name = "PPPPPP"};
                            //    return order;
                            //})
                            .Convert()
                            .ToList();

        public List<POrder> GetFullByFilter(Func<Order, bool> predicate) =>
            _orderRepository.GetFullInfo(predicate)
                            .Convert()
                            .ToList();

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
            var provider = _providerRepository.GetByName(pOrder.Provider);
            var order = _orderRepository.GetByNumberAndProvider(pOrder.Number!, provider!.Id);
            _orderRepository.Remove(order!);
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

        public IEnumerable<POrder> Get() =>
            _orderRepository.Get().Convert().ToList();

        public override POrder GetDefaultModel()
        {
            var model = base.GetDefaultModel();
            model.Date = DateTime.UtcNow;
            return model;
        }
    }
}