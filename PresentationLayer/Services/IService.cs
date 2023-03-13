using PresentationLayer.Models;
using PresentationLayer.Utils;

namespace PresentationLayer.Services
{
    public interface IService<TModel>
        where TModel : BaseModel, new()
    {
        Task<TModel> Save(TModel model);
        TModel[] GetByFilter(DataFilter<TModel> filter);
        Task Add(TModel model);
        Task Update(TModel model);
        Task Remove(TModel model);
        TModel GetDefaultModel();
    }
}