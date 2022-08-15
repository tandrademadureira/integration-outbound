using MKT.Integration.Infra.Data.Contexts;
using Shared.Infra.Data.Sql.UnitOfWork;

namespace MKT.Integration.Infra.UnitOfWork
{
    public class UnitOfWork : UnitOfWorkBase<DbContextCatalog>, IUnitOfWork
    {
        public UnitOfWork(DbContextCatalog context) : base(context) { }
    }
}
