using System.Collections;
using PresentationLayer.Models;

namespace PresentationLayer.ViewModels
{
    public class EditItemViewModel
    {
        public string? Name { get; set; }
        public decimal? Quantity { get; set; }
        public string? Unit { get; set; }
        public int OrderId { get; set; }
        public int? ItemId { get; set; }
    }
}