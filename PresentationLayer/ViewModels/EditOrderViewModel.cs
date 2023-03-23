using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PresentationLayer.Models;

namespace PresentationLayer.ViewModels
{
    public class EditOrderViewModel
    {
        public string Number { get; set; }
        public DateTime? Date { get; set; }
        public int SelectedProviderId { get; set; }
        public List<Provider> ProviderList { get; set; }
        public List<Item> Items { get; set; }
        public int? OrderId { get; set; }
    }
}
