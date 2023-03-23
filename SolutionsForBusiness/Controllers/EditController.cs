using Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;
using PresentationLayer.Services;
using PresentationLayer.ViewModels;

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
            try
            {
                var order = orderId == null ?
                    _orderService.GetDefaultModel() :
                    _orderService.GetFullByFilter(order => order.Id == orderId.Value)
                                 .FirstOrDefault() ??
                    _orderService.GetDefaultModel();

                var byFilter = orderId == null ?
                    new List<Item>() :
                    _itemService.GetByFilter(
                        new DataFilter<Infrastructure.DataBase.Models.Item, int>
                        {
                            Conditions =
                            {
                                new Condition<int>(
                                    nameof(Infrastructure.DataBase.Models.Item.OrderId),
                                    orderId.Value,
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
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("EditItem/{itemId}")]
        public IActionResult EditItem(int itemId)
        {
            try
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
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        public IActionResult EditItem(Item item)
        {
            try
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
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        public IActionResult CreateItem(EditOrderViewModel model, int orderId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Number))
                {
                    return RedirectToAction(nameof(EditOrder), orderId);
                }

                SaveOrder(model, orderId);
                orderId = _orderService.GetByNumberAndProvider(model.Number, model.SelectedProviderId)?.Id ?? orderId;
                var item = _itemService.GetDefaultModel();
                item.OrderId = orderId;
                return EditItem(item);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        public IActionResult SaveOrder(EditOrderViewModel model, int orderId)
        {
            try
            {
                var order = _orderService.GetFull(orderId) ?? _orderService.GetDefaultModel();
                var orderProvider = _providerService.Get(model.SelectedProviderId);
                order.Provider = orderProvider?.Name ?? order.Provider;
                order.Date = model.Date ?? order.Date;
                order.Number = model.Number;
                order.Items = order.Items;
                if (order.Id > 0)
                {
                    _orderService.Attach(order);
                }

                _orderService.Save(order);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        public IActionResult SaveItem(EditItemViewModel model, int orderId, int itemId)
        {
            try
            {
                var item = _itemService.Get(itemId) ?? _itemService.GetDefaultModel();
                item.OrderId = orderId;
                item.Name = model.Name;
                item.Unit = model.Unit;
                item.Quantity = model.Quantity;
                if (item.Id > 0)
                {
                    _itemService.Attach(item);
                }

                _itemService.Save(item);
                var routeValues = new
                {
                    orderId
                };

                return RedirectToAction(nameof(EditOrder), routeValues);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        public IActionResult EditProvider() => View();

        public IActionResult SaveProvider(string providerName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(providerName))
                {
                    return BadRequest("Empty name");
                }

                var provider = _providerService.GetDefaultModel();
                provider.Name = providerName;
                _providerService.Save(provider);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        public IActionResult RemoveItem(int itemId, int orderId)
        {
            try
            {
                Item model = new()
                {
                    Id = itemId
                };

                _itemService.Detached(model);
                _itemService.Save(model);

                return RedirectToAction(nameof(EditOrder), new
                {
                    orderId
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        public IActionResult RemoveOrder(int orderId)
        {
            try
            {
                var order = new Order
                {
                    Id = orderId
                };

                _orderService.Detached(order);
                _orderService.Save(order);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}