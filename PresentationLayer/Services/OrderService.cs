using Infrastructure.DataBase.Models;
using Infrastructure.Repositories;
using PresentationLayer.Extensions;
using PresentationLayer.Utils;
using POrder = PresentationLayer.Models.Order;
using PProvider = PresentationLayer.Models.Provider;
using PItem = PresentationLayer.Models.OrderItem;
using PresentationLayer.Enums;

namespace PresentationLayer.Services
{
    public class OrderService : BaseService<POrder>
    {
        private readonly OrderRepository _orderRepository;
        private readonly ProviderRepository _providerRepository;

        public OrderService(OrderRepository orderRepository, ProviderRepository providerRepository)
        {
            _orderRepository = orderRepository;
            _providerRepository = providerRepository;
        }

        public override async Task<POrder> Save(POrder model)
        {
            //var provider = await _providerRepository.GetByName(model.Provider);
            //if (provider == null)
            //{
            //    throw new ArgumentException($"There is no provider with name {model.Provider}");
            //}

            //if (!model.Items?.Any() != true)
            //{
            //    throw new ArgumentException("Order must contain at least one item");
            //}

            return await base.Save(model);
        }

        public override POrder[] GetByFilter(DataFilter<POrder> filter)
        {
            return _orderRepository.GetByNumberAndProvider(order => filter.Filter(order.Convert()))
                                   .Select(Converter.Convert)
                                   .ToArray();
        }

        public override async Task Add(POrder model)
        {
            var provider = _providerRepository.GetByName(model.Provider);
            provider ??= new Provider
            {
                Name = model.Provider
            };

            var entity = model.Convert(provider);
            _orderRepository.Add(entity);
        }

        public override async Task Update(POrder model)
        {
            await _orderRepository.UpdateAsync(model.Convert(null));
        }

        public override async Task Remove(POrder pOrder)
        {
            var provider = _providerRepository.GetByName(pOrder.Provider);
            var order = await _orderRepository.GetByNumberAndProvider(pOrder.Number!, provider!.Id);
            await _orderRepository.Remove(order!);
        }

        public async Task<POrder> Create(string? number, string provider, IEnumerable<PItem> items)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentNullException(nameof(number));
            }

            var order = GetDefaultModel();
            order.Number = number;
            order.Items = items;
            order.Provider = provider;

            return await Save(order);
        }

        public List<IEnumerable<Order>> Get() => _orderRepository.Get().Paginate().ToList();

        public override POrder GetDefaultModel()
        {
            var model = base.GetDefaultModel();
            model.Date = DateTime.UtcNow;
            return model;
        }
    }
}