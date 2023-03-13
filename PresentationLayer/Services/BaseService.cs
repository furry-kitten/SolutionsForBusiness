using PresentationLayer.Enums;
using PresentationLayer.Models;
using PresentationLayer.Utils;

namespace PresentationLayer.Services
{
    public abstract class BaseService<TModel> : IService<TModel>
        where TModel : BaseModel, new()
    {
        public virtual async Task<TModel> Save(TModel model)
        {
            switch (model.State)
            {
                case ModelState.Added:
                    await Add(model);
                    break;
                case ModelState.Modified:
                    await Update(model);
                    break;
                case ModelState.Detached:
                    await Remove(model);
                    break;
            }

            return model;
        }

        public abstract TModel[] GetByFilter(DataFilter<TModel> filter);

        public abstract Task Add(TModel model);

        public abstract Task Update(TModel model);

        public abstract Task Remove(TModel model);

        public virtual TModel GetDefaultModel() =>
            new ()
            {
                State = ModelState.Added
            };
    }
}