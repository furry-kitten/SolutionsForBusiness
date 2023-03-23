using Infrastructure.DataBase.Models;
using PresentationLayer.Enums;
using POrder = PresentationLayer.Models.Order;
using PProvider = PresentationLayer.Models.Provider;
using PItem = PresentationLayer.Models.Item;

namespace PresentationLayer.Extensions
{
    internal static class Converter
    {
        internal static POrder Convert(this Order entity) =>
            new()
            {
                Id = entity.Id,
                Provider = entity.Provider?.Name,
                Items = entity.Items?.Convert(),
                Date = entity.Date,
                Number = entity.Number,
                State = ModelState.None
            };

        internal static Order Convert(this POrder entity, Provider? provider)
        {
            var order = new Order
            {
                Provider = provider,
                Date = entity.Date ?? DateTime.UtcNow,
                Number = entity.Number,
                ProviderId = provider?.Id
            };

            if(entity.Id > 0 )
            {
                order.Id = entity.Id;
            }

            order.Items = entity.Items?.Convert(order).ToList();
            return order;
        }

        internal static PItem Convert(this Item entity) =>
            new()
            {
                Id = entity.Id,
                OrderNumber = entity.Order?.Number,
                Name = entity.Name,
                Quantity = entity.Quantity,
                Unit = entity.Unit,
                OrderId = entity.OrderId,
                State = ModelState.None
            };

        internal static Item Convert(this PItem entity, Order? order)
        {
            Item convert = new()
            {
                Name = entity.Name,
                Quantity = entity.Quantity,
                Unit = entity.Unit,
                OrderId = entity.OrderId
            };

            if (entity.Id > 0)
            {
                convert.Id = entity.Id;
            }

            return convert;
        }

        internal static PProvider Convert(this Provider entity) =>
            new()
            {
                Id = entity.Id,
                Name = entity.Name,
                Orders = entity.Orders?.Convert(),
                State = ModelState.None
            };

        internal static Provider Convert(this PProvider entity)
        {
            Provider provider = new()
            {
                Name = entity.Name
            };

            if (entity.Id > 0)
            {
                provider.Id = entity.Id;
            }

            return provider;
        }

        internal static IEnumerable<PProvider> Convert(this IEnumerable<Provider> enumerable)
        {
            return enumerable.Select(Convert);
        }

        internal static IEnumerable<Provider> Convert(this IEnumerable<PProvider> enumerable)
        {
            return enumerable.Select(Convert);
        }

        internal static IEnumerable<POrder> Convert(this IEnumerable<Order> enumerable)
        {
            return enumerable.Select(Convert);
        }

        internal static IEnumerable<Order> Convert(this IEnumerable<POrder> enumerable, Provider provider)
        {
            return enumerable.Select(order => order.Convert(provider));
        }

        internal static IEnumerable<PItem> Convert(this IEnumerable<Item> enumerable)
        {
            return enumerable.Select(Convert);
        }

        internal static IEnumerable<Item> Convert(this IEnumerable<PItem> enumerable, Order order)
        {
            return enumerable.Select(item => item.Convert(order));
        }
    }
}