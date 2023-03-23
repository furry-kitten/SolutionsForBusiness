using PresentationLayer.Enums;

namespace PresentationLayer.Models
{
    public abstract class BaseModel
    {
        public int Id { get; init; }
        internal ModelState State { get; set; } = ModelState.None;
    }
}
