using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MKT.Integration.Domain.AggregateModels.Products;
using System;

namespace MKT.Integration.Infra.Data.Mappings
{
    internal sealed class CatalogMapping
    {
        internal static Action<EntityTypeBuilder<Product>> ConfigureMap()
        {
            return (entity) =>
            {
                entity.ToTable("Catalog");
                entity.HasKey(it => it.Id);
                entity.Property(it => it.Description).HasColumnName("Description").HasColumnType("varchar(50)").IsRequired();
                entity.Property(it => it.Mark).HasColumnName("Mark").HasColumnType("varchar(50)").IsRequired();
                entity.Property(it => it.Approved).HasColumnName("Approved").HasColumnType("bit").IsRequired();
                entity.Property(it => it.Amount).HasColumnName("Amount").HasColumnType("int").IsRequired();
            };
        }
    }
}