using Infrastructure.DataBase;
using Infrastructure.DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProviderRepository : BaseRepository<Provider>
    {
        public ProviderRepository(EfCoreContext context) : base(context) { }

        public Provider? GetByName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            var firstOrDefaultAsync = WriteEntity.AsNoTracking().FirstOrDefault(provider =>
                string.IsNullOrWhiteSpace(provider.Name) == false &&
                provider.Name.Equals(name) == true);

            return firstOrDefaultAsync;
        }
    }
}