using Infrastructure.DataBase.Models;
using Infrastructure.Utils;
using PresentationLayer.Enums;
using PresentationLayer.Models;
using Order = PresentationLayer.Models.Order;

namespace PresentationLayer.Services
{
    public abstract class BaseService<TEntity, TModel> : IService<TEntity, TModel>
        where TEntity : BaseDbModel, new()
        where TModel : BaseModel, new()
    {
        public virtual TModel Save(TModel model)
        {
            switch (model.State)
            {
                case ModelState.Added:
                    Add(model);
                    break;
                case ModelState.Modified:
                    Update(model);
                    break;
                case ModelState.Detached:
                    Remove(model);
                    break;
            }

            return model;
        }

        public abstract List<TModel> GetByFilter<TEntityType>(DataFilter<TEntity, TEntityType> filter);

        public abstract void Add(TModel model);

        public abstract void Update(TModel model);

        public abstract void Remove(TModel model);

        public virtual TModel GetDefaultModel() =>
            new ()
            {
                State = ModelState.Added
            };

        public virtual void Attach(TModel model)
        {
            model.State = ModelState.Modified;
        }

        public virtual void Detached(TModel model)
        {
            model.State = ModelState.Detached;
        }

        public abstract TModel? Get(int id);

        public abstract List<TModel> GetFullByFilter<TEntityType>(DataFilter<TEntity, TEntityType> filter);
    }
}