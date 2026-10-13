using ControlElectricoObrera.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlElectricoObrera.Api.Data.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(product => product.Id);

        builder.Property(product => product.Name).HasMaxLength(200).IsRequired();
        builder.Property(product => product.Slug).HasMaxLength(220).IsRequired();
        builder.Property(product => product.ShortDescription).HasMaxLength(500);
        builder.Property(product => product.Description).HasColumnType("text");
        builder.Property(product => product.Model).HasMaxLength(200);
        builder.Property(product => product.SkuOrCode).HasMaxLength(200);
        builder.Property(product => product.Unit).HasMaxLength(50).IsRequired();
        builder.Property(product => product.AvailabilityStatus)
            .HasConversion<int>()
            .HasDefaultValue(ProductAvailabilityStatus.Consult);
        builder.Property(product => product.IsFeatured).HasDefaultValue(false);
        builder.Property(product => product.IsActive).HasDefaultValue(true);
        builder.Property(product => product.SeoTitle).HasMaxLength(200);
        builder.Property(product => product.SeoDescription).HasMaxLength(500);
        builder.Property(product => product.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(product => product.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(product => product.Slug).IsUnique();
        builder.HasIndex(product => product.Name);
        builder.HasIndex(product => product.BrandId);
        builder.HasIndex(product => product.CategoryId);
        builder.HasIndex(product => product.IsActive);

        builder.HasOne(product => product.Brand)
            .WithMany(brand => brand.Products)
            .HasForeignKey(product => product.BrandId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(product => product.Category)
            .WithMany(category => category.Products)
            .HasForeignKey(product => product.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
