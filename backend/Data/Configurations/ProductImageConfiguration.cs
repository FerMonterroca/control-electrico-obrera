using ControlElectricoObrera.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlElectricoObrera.Api.Data.Configurations;

public sealed class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.HasKey(image => image.Id);

        builder.Property(image => image.StorageKey).HasMaxLength(1000).IsRequired();
        builder.Property(image => image.AltText).HasMaxLength(300).IsRequired();
        builder.Property(image => image.SortOrder).HasDefaultValue(0);
        builder.Property(image => image.IsPrimary).HasDefaultValue(false);
        builder.Property(image => image.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(image => image.ProductId);
        builder.HasIndex(image => new { image.ProductId, image.SortOrder });

        builder.HasOne(image => image.Product)
            .WithMany(product => product.Images)
            .HasForeignKey(image => image.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
