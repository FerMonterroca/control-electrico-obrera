using ControlElectricoObrera.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlElectricoObrera.Api.Data.Configurations;

public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(category => category.Id);

        builder.Property(category => category.Name).HasMaxLength(200).IsRequired();
        builder.Property(category => category.Slug).HasMaxLength(220).IsRequired();
        builder.Property(category => category.Description).HasColumnType("text");
        builder.Property(category => category.ImageStorageKey).HasMaxLength(1000);
        builder.Property(category => category.IsActive).HasDefaultValue(true);
        builder.Property(category => category.SeoTitle).HasMaxLength(200);
        builder.Property(category => category.SeoDescription).HasMaxLength(500);
        builder.Property(category => category.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(category => category.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(category => category.Slug).IsUnique();
        builder.HasIndex(category => category.ParentCategoryId);

        builder.HasOne(category => category.ParentCategory)
            .WithMany(category => category.ChildCategories)
            .HasForeignKey(category => category.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
