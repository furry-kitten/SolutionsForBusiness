using Infrastructure.DataBase.Models;
using Infrastructure.Utils;
using PresentationLayer.Models;

namespace PresentationLayer.Services
{
    public interface IService<TEntity, TModel>
        where TEntity : BaseDbModel, new()
        where TModel : BaseModel, new()
    {
        public TModel? Get(int id);
        public TModel Save(TModel model);
        public List<TModel> GetByFilter<TEntityType>(DataFilter<TEntity, TEntityType> filter);
        public void Add(TModel model);
        public void Update(TModel model);
        public void Remove(TModel model);
        public TModel GetDefaultModel();
        public void Attach(TModel model);
        public void Detached(TModel model);
    }
}