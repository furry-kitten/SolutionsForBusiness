using Infrastructure.Utils;
using PresentationLayer.Models;

namespace PresentationLayer.Services
{
    public interface IProviderService : IService<Infrastructure.DataBase.Models.Provider, Provider>
    {
        Task<Provider> Create(string name);
        List<Provider> Get();
        Provider? GetByName(string name);
        List<Provider> GetFullByFilter<TEntityType>(DataFilter<Infrastructure.DataBase.Models.Provider, TEntityType> filter);
    }
}