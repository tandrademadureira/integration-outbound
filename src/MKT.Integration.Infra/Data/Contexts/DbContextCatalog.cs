using Microsoft.EntityFrameworkCore;
using MKT.Integration.Domain.AggregateModels.Products;
using MKT.Integration.Infra.Data.Mappings;
using Shared.Infra.Data.Sql.Contexts;

namespace MKT.Integration.Infra.Data.Contexts
{
    public class DbContextCatalog : DbContextBase, IDbContext
    {
        public DbContextCatalog(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Product> Catalog { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity(CatalogMapping.ConfigureMap());
        }
    }
}