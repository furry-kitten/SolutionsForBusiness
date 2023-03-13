using PresentationLayer.Enums;
using PresentationLayer.Models;

namespace PresentationLayer.Extensions
{
    public static class Extensions
    {
        public static void Update(this BaseModel model) => model.State = ModelState.Modified;
        public static void Detach(this BaseModel model) => model.State = ModelState.Detached;
    }
}
