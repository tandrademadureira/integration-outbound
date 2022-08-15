using MKT.Integration.Domain.AggregateModels.Products;
using MKT.Integration.Infra.Data.Contexts;
using Shared.Infra.Data.Sql.Extensions;
using Shared.Infra.Data.Sql.Repository;
using Shared.Util.Extension;
using System.Linq;
using System.Threading.Tasks;

namespace MKT.Integration.Infra.Data.Repositories
{
    public class ProductRepository : RepositoryBase<DbContextCatalog, Product>, IProductRepository
    {
        public ProductRepository(DbContextCatalog context) : base(context) { }

        public async Task SetAutoChanges(bool value)
        {
            _context.SetAutoDetectChanges(value);
            await Task.CompletedTask;
        }

        async Task IProductRepository.UpdateCatalog(Product catalog)
        {
            _context.SetModified(catalog);
            await Task.CompletedTask;
        }
    }
}
