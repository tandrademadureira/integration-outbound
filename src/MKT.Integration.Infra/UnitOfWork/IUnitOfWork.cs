using Shared.Infra.Data.Sql.Contexts;
using Shared.Infra.Data.Sql.UnitOfWork;

namespace MKT.Integration.Infra.UnitOfWork
{
    public interface IUnitOfWork : IUnitOfWork<IDbContext>
    {
    }
}
