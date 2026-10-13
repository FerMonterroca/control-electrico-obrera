using ControlElectricoObrera.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlElectricoObrera.Api.Data.Configurations;

public sealed class ProductSpecificationConfiguration : IEntityTypeConfiguration<ProductSpecification>
{
    public void Configure(EntityTypeBuilder<ProductSpecification> builder)
    {
        builder.HasKey(specification => specification.Id);

        builder.Property(specification => specification.Name).HasMaxLength(200).IsRequired();
        builder.Property(specification => specification.Value).HasMaxLength(500).IsRequired();
        builder.Property(specification => specification.Unit).HasMaxLength(50);
        builder.Property(specification => specification.SortOrder).HasDefaultValue(0);

        builder.HasIndex(specification => specification.ProductId);
        builder.HasIndex(specification => new { specification.ProductId, specification.SortOrder });

        builder.HasOne(specification => specification.Product)
            .WithMany(product => product.Specifications)
            .HasForeignKey(specification => specification.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
