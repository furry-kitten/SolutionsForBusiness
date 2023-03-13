using Infrastructure.DataBase.Models;
using PresentationLayer.Enums;
using POrder = PresentationLayer.Models.Order;
using PProvider = PresentationLayer.Models.Provider;
using PItem = PresentationLayer.Models.OrderItem;

namespace PresentationLayer.Extensions
{
    internal static class Converter
    {
        internal static POrder Convert(this Order entity) =>
            new()
            {
                Id = entity.Id,
                Provider = entity.Provider?.Name,
                Items = entity.Items?.Select(Convert),
                Date = entity.Date,
                Number = entity.Number,
                State = ModelState.None
            };

        internal static Order Convert(this POrder entity, Provider? provider)
        {
            var order = new Order
            {
                Id = entity.State == ModelState.None ? 0 : entity.Id,
                Provider = provider,
                Date = entity.Date ?? DateTime.UtcNow,
                Number = entity.Number
            };

            order.Items = entity.Items?.Select(item => Convert(item, order)).ToList();
            return order;
        }

        internal static PItem Convert(this OrderItem entity) =>
            new()
            {
                Id = entity.Id,
                Order = entity.Order?.Number,
                Name = entity.Name,
                Quantity = entity.Quantity,
                Unit = entity.Unit,
                State = ModelState.None
            };

        internal static OrderItem Convert(this PItem entity, Order? order) =>
            new()
            {
                Id = entity.State == ModelState.None ? 0 : entity.Id,
                Order = order,
                Name = entity.Name,
                Quantity = entity.Quantity,
                Unit = entity.Unit
            };

        internal static PProvider Convert(this Provider entity) =>
            new()
            {
                Id = entity.Id,
                Name = entity.Name,
                Orders = entity.Orders?.Select(Convert),
                State = ModelState.None
            };

        internal static Provider Convert(this PProvider entity) =>
            new()
            {
                Id = entity.State == ModelState.None ? 0 : entity.Id,
                Name = entity.Name
            };
    }
}