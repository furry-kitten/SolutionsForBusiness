using Infrastructure.DataBase.Models;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Services;
using PresentationLayer.ViewModels;
using Order = PresentationLayer.Models.Order;

namespace SolutionsForBusiness.Controllers
{
    public class EditController : Controller
    {
        private readonly IItemService _itemService;
        private readonly IOrderService _orderService;
        private readonly IProviderService _providerService;

        public EditController(IOrderService orderService,
            IProviderService providerService,
            IItemService itemService)
        {
            _itemService = itemService;
            _orderService = orderService;
            _providerService = providerService;
        }

        public IActionResult Index() => View();

        public IActionResult EditOrder(int? orderId)
        {
            var order = orderId == null ?
                _orderService.GetDefaultModel() :
                _orderService.GetFullByFilter(order => order.Id == orderId.Value)
                             .FirstOrDefault() ??
                _orderService.GetDefaultModel();

            var byFilter = orderId == null ?
                new List<PresentationLayer.Models.Item>() :
                _itemService.GetByFilter(new DataFilter<Item, int>
                {
                    Conditions =
                    {
                        new Condition<int>(nameof(Item.OrderId), orderId.Value,
                            tuple => tuple.entityValue == tuple.conditionValue)
                    }
                });

            var providerList = _providerService.Get();
            var viewModel = new EditOrderViewModel
            {
                Date = order.Date,
                Number = order.Number,
                OrderId = order.Id,
                ProviderList = providerList,
                Items = byFilter,
                SelectedProviderId = (orderId == null ?
                                         providerList.FirstOrDefault()?.Id :
                                         _providerService.GetByName(order.Provider!)?.Id) ??
                                     0
            };

            return View(viewModel);
        }

        [HttpGet("EditItem/{itemId}")]
        public IActionResult EditItem(int itemId)
        {
            var item = _itemService.Get(itemId);
            EditItemViewModel model = new()
            {
                ItemId = itemId,
                OrderId = item!.OrderId!.Value,
                Name = item.Name,
                Quantity = item.Quantity,
                Unit = item.Unit
            };

            return View(model);
        }

        public IActionResult EditItem(PresentationLayer.Models.Item item)
        {
            EditItemViewModel model = new()
            {
                Name = item.Name,
                Unit = item.Unit,
                Quantity = item.Quantity,
                ItemId = item.Id,
                OrderId = item.OrderId ?? 0
            };

            return View("EditItem", model);
        }

        public IActionResult CreateItem(EditOrderViewModel model, int orderId)
        {
            if (string.IsNullOrWhiteSpace(model.Number))
            {
                return RedirectToAction(nameof(EditOrder), orderId);
            }

            SaveOrder(model, orderId);
            var item = _itemService.GetDefaultModel();
            item.OrderId = orderId;
            return EditItem(item);
        }

        public IActionResult SaveOrder(EditOrderViewModel model, int orderId)
        {
            var order = _orderService.GetFull(orderId) ?? _orderService.GetDefaultModel();
            var orderProvider = _providerService.Get(model.SelectedProviderId);
            order.Provider = orderProvider?.Name ?? order.Provider;
            order.Date = model.Date ?? order.Date;
            order.Number = model.Number;
            order.Items = order.Items;
            _orderService.Attach(order);
            _orderService.Save(order);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public IActionResult SaveItem(EditItemViewModel model, int orderId, int itemId)
        {
            var item = _itemService.Get(itemId) ?? _itemService.GetDefaultModel();
            item.OrderId = orderId;
            item.Name = model.Name;
            item.Unit = model.Unit;
            item.Quantity = model.Quantity;
            if (item.Id == 0)
            {
                var order = _orderService.GetFull(orderId);
                _orderService.Attach(order);
                var orderItems = order.Items.ToList();
                orderItems.Add(item);
                order.Items = orderItems;
                _orderService.Save(order);
            }
            else
            {
                _itemService.Attach(item);
                _itemService.Save(item);
            }

            return RedirectToAction(nameof(EditOrder), new
            {
                orderId = orderId
            });
        }
    }
}