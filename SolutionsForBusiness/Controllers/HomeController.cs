using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Services;
using PresentationLayer.Models;
using SolutionsForBusiness.Models;

namespace SolutionsForBusiness.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IService<Order> orderService, IService<OrderItem> itemService)
        {
            _logger = logger;
            var order = orderService.GetDefaultModel();
            order.Provider = "MTC";
            order.Number = "753";

            var item1 = itemService.GetDefaultModel();
            item1.Order = order.Number;
            item1.Quantity = (decimal?)265.5;
            item1.Name = nameof(item1);
            item1.Unit = "Материал1";

            var item2 = itemService.GetDefaultModel();
            item2.Order = order.Number;
            item2.Quantity = (decimal?)365.8;
            item2.Name = nameof(item2);
            item2.Unit = "Материал2";

            var item3 = itemService.GetDefaultModel();
            item3.Order = order.Number;
            item3.Quantity = (decimal?)1265.5;
            item3.Name = nameof(item3);
            item3.Unit = "Материал3";

            order.Items = new[]
            {
                item1,
                item2,
                item3
            };

            Task.Run(async () =>
            {
                try
                {
                    await orderService.Save(order);
                }
                catch (Exception e)
                {
                    
                }
            });
        }

        public IActionResult Index() => View();

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
    }
}