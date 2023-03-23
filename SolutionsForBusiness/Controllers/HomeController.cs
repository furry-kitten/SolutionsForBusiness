using System.Diagnostics;
using Infrastructure.DataBase.Models;
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

        public HomeController(ILogger<HomeController> logger,
            IOrderService orderService,
            IProviderService providerService)
        {
            _logger = logger;
            _orderService = orderService;
            _providerService = providerService;
        }

        public IActionResult Index(HistoryViewModel filters)
        {
            try
            {
                var dateName = nameof(Order.Date);
                DataFilter<Order, string>? numberFilter = null;
                DataFilter<Order, int>? providerFilter = null;

                filters.LastDateFilter =
                    filters.LastDateFilter != default &&
                    filters.LastDateFilter.Date <= DateTime.Now.Date ?
                        filters.LastDateFilter :
                        DateTime.UtcNow.Date;

                filters.FirstDateFilter =
                    filters.LastDateFilter > filters.FirstDateFilter &&
                    filters.FirstDateFilter != default ?
                        filters.FirstDateFilter :
                        DateTime.UtcNow.AddMonths(-1).Date;

                DataFilter<Order, DateTime> dateFilter = new()
                {
                    Conditions =
                    {
                        new Condition<DateTime>(dateName, filters.LastDateFilter,
                            tuple => tuple.entityValue <= tuple.conditionValue.AddDays(1))
                            {
                                AddCondition = new Condition<DateTime>(dateName,
                                    filters.FirstDateFilter,
                                    tuple => tuple.entityValue > tuple.conditionValue)
                            }
                    }
                };

                if (filters.NumberFilter?.Any() == true)
                {
                    numberFilter = new DataFilter<Order, string>();
                    numberFilter.Conditions.AddRange(filters.NumberFilter.Select(orderNumber =>
                        new Condition<string>(nameof(Order.Number), orderNumber,
                            tuple => tuple.entityValue?.Equals(tuple.conditionValue) == true)));
                }

                if (filters.ProvidersIdFilter?.Any() == true)
                {
                    providerFilter = new DataFilter<Order, int>();
                    providerFilter.Conditions.AddRange(filters.ProvidersIdFilter.Select(
                        orderProvider => new Condition<int>(nameof(Order.ProviderId), orderProvider,
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
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
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