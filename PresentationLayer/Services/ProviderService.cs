using Infrastructure.DataBase.Models;
using Infrastructure.Repositories;
using Infrastructure.Utils;
using PresentationLayer.Enums;
using PresentationLayer.Extensions;
using POrder = PresentationLayer.Models.Order;
using PProvider = PresentationLayer.Models.Provider;
using PItem = PresentationLayer.Models.Item;

namespace PresentationLayer.Services
{
    public class ProviderService : BaseService<Provider, PProvider>, IProviderService
    {
        private readonly IProviderRepository _providerRepository;

        public ProviderService(IProviderRepository providerRepository) =>
            _providerRepository = providerRepository;

        //public static ProviderWorker Instance { get; } = new();

        public async Task<PProvider> Create(string name)
        {
            var existsProvider = _providerRepository.Get<Provider>(provider =>
                                                        string.IsNullOrWhiteSpace(provider.Name) ==
                                                        false &&
                                                        provider.Name.Equals(name,
                                                            StringComparison
                                                                .CurrentCultureIgnoreCase))
                                                    .FirstOrDefault();

            if (existsProvider is not null)
            {
                return existsProvider.Convert();
            }

            return Save(new PProvider
            {
                Name = name,
                State = ModelState.Added
            });
        }

        public List<PProvider> Get() => _providerRepository.Get().Convert().ToList();

        public override PProvider? Get(int id) => _providerRepository.Get(id)?.Convert();

        public PProvider? GetByName(string name) =>
            _providerRepository.GetByName(name)?.Convert();

        public override List<PProvider>
            GetByFilter<TEntityType>(DataFilter<Provider, TEntityType> filter) =>
            _providerRepository.Get(filter).Convert().ToList();

        public override void Add(PProvider model)
        {
            if (_providerRepository.GetByName(model.Name) != null)
            {
                return;
            }

            _providerRepository.Add(model.Convert());
        }

        public override void Update(PProvider order)
        {
            _providerRepository.Update(order.Convert());
        }

        public override async void Remove(PProvider model)
        {
            await _providerRepository.Remove(new Provider
            {
                Id = model.Id
            });
        }
        public override List<PProvider> GetFullByFilter<TEntityType>(DataFilter<Provider, TEntityType> filter)
        {
            return _providerRepository.Get(filter).Convert().ToList();
        }
    }
}