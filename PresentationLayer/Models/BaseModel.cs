using PresentationLayer.Enums;

namespace PresentationLayer.Models
{
    public abstract class BaseModel
    {
        internal int Id { get; init; } = -1;
        internal ModelState State { get; set; } = ModelState.None;
    }
}
