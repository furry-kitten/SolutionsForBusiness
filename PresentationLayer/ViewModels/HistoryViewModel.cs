using System.ComponentModel.DataAnnotations;
using PresentationLayer.Models;

namespace PresentationLayer.ViewModels
{
    public class HistoryViewModel
    {
        public IEnumerable<Order>? History { get; set; }
        public IEnumerable<Provider>? Providers { get; set; }

        #region filter fields
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateTime FirstDateFilter { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateTime LastDateFilter { get; set; }

        public List<int>? ProvidersIdFilter { get; set; }

        public List<string>? NumberFilter { get; set; }
        public List<string?>? AllNumbers { get; set; }

        #endregion
    }
}
