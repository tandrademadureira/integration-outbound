using MKT.Integration.Domain.AggregateModels.Products;
using Shared.Infra.Data.Sql.Repository;
using Shared.Util.Extension;
using System.Threading.Tasks;

namespace MKT.Integration.Infra.Data.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task SetAutoChanges(bool value);
        Task UpdateCatalog(Product catalog);
    }
}
