using ControlElectricoObrera.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlElectricoObrera.Api.Data.Configurations;

public sealed class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.HasKey(brand => brand.Id);

        builder.Property(brand => brand.Name).HasMaxLength(200).IsRequired();
        builder.Property(brand => brand.Slug).HasMaxLength(220).IsRequired();
        builder.Property(brand => brand.Description).HasColumnType("text");
        builder.Property(brand => brand.LogoStorageKey).HasMaxLength(1000);
        builder.Property(brand => brand.IsActive).HasDefaultValue(true);
        builder.Property(brand => brand.SeoTitle).HasMaxLength(200);
        builder.Property(brand => brand.SeoDescription).HasMaxLength(500);
        builder.Property(brand => brand.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(brand => brand.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(brand => brand.Slug).IsUnique();
    }
}
