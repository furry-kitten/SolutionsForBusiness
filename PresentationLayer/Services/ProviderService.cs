using Infrastructure.DataBase.Models;
using Infrastructure.Repositories;
using PresentationLayer.Enums;
using PresentationLayer.Extensions;
using PresentationLayer.Utils;
using POrder = PresentationLayer.Models.Order;
using PProvider = PresentationLayer.Models.Provider;
using PItem = PresentationLayer.Models.OrderItem;

namespace PresentationLayer.Services
{
    public class ProviderService : BaseService<PProvider>
    {
        private readonly ProviderRepository _providerRepository;

        public ProviderService(ProviderRepository providerRepository) =>
            _providerRepository = providerRepository;

        //public static ProviderWorker Instance { get; } = new();

        public async Task<PProvider> Create(string name)
        {
            var existsProvider = _providerRepository.Get(provider => string.IsNullOrWhiteSpace(provider.Name) == false &&
                                                        provider.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                                                    .FirstOrDefault();

            if (existsProvider is not null)
            {
                return existsProvider.Convert();
            }

            return await Save(new PProvider
            {
                Name = name,
                State = ModelState.Added
            });
        }

        public List<PProvider> Get() =>
            _providerRepository.Get().Select(Converter.Convert).ToList();

        public override PProvider[] GetByFilter(DataFilter<PProvider> filter)
        {
            return _providerRepository.Get(provider => filter.Filter(provider.Convert()))
                                      .Select(Converter.Convert)
                                      .ToArray();
        }
        public override async Task Add(PProvider model)
        {
            await _providerRepository.AddAsync(model.Convert());
        }

        public override async Task Update(PProvider order)
        {
            await _providerRepository.UpdateAsync(order.Convert());
        }

        public override async Task Remove(PProvider model)
        {
            await _providerRepository.Remove(new Provider
            {
                Id = model.Id
            });
        }
    }
}