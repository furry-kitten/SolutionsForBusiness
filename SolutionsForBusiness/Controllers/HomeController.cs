using System.Diagnostics;
using Infrastructure.DataBase.Models;
using Infrastructure.Extensions;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Services;
using PresentationLayer.ViewModels;
using SolutionsForBusiness.Models;

namespace SolutionsForBusiness.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProviderService _providerService;
        private readonly IOrderService _orderService;
        private readonly IItemService _itemService;

        public HomeController(ILogger<HomeController> logger,
            IOrderService orderService,
            IItemService itemService,
            IProviderService providerService)
        {
            _logger = logger;
            _orderService = orderService;
            _itemService = itemService;
            _providerService = providerService;
        }

        public IActionResult Index(HistoryViewModel filters)
        {
            var dateName = nameof(Order.Date);
            DataFilter<Order, string> numberFilter = null;
            DataFilter<Order, int> providerFilter = null;

            filters.LastDateFilter = filters.LastDateFilter != default && filters.LastDateFilter.Date <= DateTime.Now.Date? filters.LastDateFilter : DateTime.UtcNow.Date;
            filters.FirstDateFilter = filters.LastDateFilter > filters.FirstDateFilter && filters.FirstDateFilter != default ?
                filters.FirstDateFilter :
                DateTime.UtcNow.AddMonths(-1).Date;

            DataFilter<Order, DateTime> dateFilter = new()
            {
                Conditions =
                {
                    new Condition<DateTime>(dateName, filters.LastDateFilter,
                        tuple => tuple.entityValue <= tuple.conditionValue)
                        {
                            AddCondition = new Condition<DateTime>(dateName, filters.FirstDateFilter,
                                tuple => tuple.entityValue > tuple.conditionValue)
                        }
                }
            };

            if (filters.NumberFilter?.Any() == true)
            {
                numberFilter = new DataFilter<Order, string>();
                numberFilter.Conditions.AddRange(filters.NumberFilter.Select(orderNumber =>
                    new Condition<string>(nameof(Order.Number), orderNumber,
                        tuple => tuple.entityValue.Equals(tuple.conditionValue))));
            }

            if (filters.ProvidersIdFilter?.Any() == true)
            {
                providerFilter = new DataFilter<Order, int>();
                providerFilter.Conditions.AddRange(filters.ProvidersIdFilter.Select(orderProvider =>
                    new Condition<int>(nameof(Order.ProviderId), orderProvider,
                        tuple => tuple.entityValue.Equals(tuple.conditionValue))));
            }

            var byFilter = _orderService.GetFullByFilter(order =>
                dateFilter.Filter(order) &&
                numberFilter?.Filter(order) != false &&
                providerFilter?.Filter(order) != false);
            var providers = _providerService.Get();
            var historyViewModel = new HistoryViewModel
            {
                History = byFilter,
                Providers = providers,
                AllNumbers = _orderService.Get().Select(order => order.Number).ToList(),
                FirstDateFilter = filters.FirstDateFilter,
                LastDateFilter = filters.LastDateFilter,
                NumberFilter = new List<string>(),
                ProvidersIdFilter = new List<int>()
            };

            return View(historyViewModel);
        }

        public IActionResult FilterIndex(HistoryViewModel filters)
        {
            var dateName = nameof(Order.Date);
            DataFilter<Order, string> numberFilter = null;
            DataFilter<Order, int> providerFilter = null;
            DataFilter<Order, DateTime> dateFilter = new()
            {
                Conditions =
                {
                    new Condition<DateTime>(dateName, DateTime.UtcNow.Date,
                        tuple => tuple.entityValue <= tuple.conditionValue)
                        {
                            AddCondition = new Condition<DateTime>(dateName,
                                DateTime.UtcNow.AddMonths(-1).Date,
                                tuple => tuple.entityValue > tuple.conditionValue)
                        }
                }
            };

            var condition = dateFilter.Conditions.First();
            //if (filters.FirstDateFilter != default && filters.LastDateFilter != default)
            //{
            //    condition.ChangeDates(filters.LastDateFilter, filters.FirstDateFilter);
            //}
            //else if (filters.FirstDateFilter != default)
            //{
            //    condition.ChangeDates(condition.AddCondition?.Value ?? DateTime.Now, filters.FirstDateFilter);
            //}
            //else if (filters.LastDateFilter != default)
            //{
            //    condition.ChangeDates(filters.LastDateFilter, condition.Value);
            //}

            //if (filters.NumberFilter?.Any() == true)
            //{
            //    numberFilter = new DataFilter<Order, string>();
            //    numberFilter.Conditions.AddRange(filters.NumberFilter.Select(orderNumber => new Condition<string>(nameof(Order.Number), orderNumber, tuple => tuple.entityValue.Equals(tuple.conditionValue))));
            //}

            //if (filters.ProvidersFilter?.Any() == true)
            //{
            //   providerFilter = new DataFilter<Order, int>();
            //   providerFilter.Conditions.AddRange(filters.Provider.Select(orderProvider => new Condition<int>(nameof(Order.ProviderId), orderProvider.Id, tuple => tuple.entityValue.Equals(tuple.conditionValue))));
            //}

            var byFilter = _orderService.GetFullByFilter(order =>
                dateFilter.Filter(order) ||
                numberFilter?.Filter(order) != false ||
                providerFilter?.Filter(order) != false);
            var historyViewModel = new HistoryViewModel
            {
                History = byFilter,
                Providers = _providerService.Get(),
                FirstDateFilter = DateTime.Now.Date,
                LastDateFilter = DateTime.Now.AddMonths(-1).Date,
                NumberFilter = new List<string>(),
                ProvidersIdFilter = new List<int>()
            };

            return View(nameof(Index), historyViewModel);
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
    }
}