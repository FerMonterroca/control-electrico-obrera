using ControlElectricoObrera.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlElectricoObrera.Api.Data.Configurations;

public sealed class QuoteItemConfiguration : IEntityTypeConfiguration<QuoteItem>
{
    public void Configure(EntityTypeBuilder<QuoteItem> builder)
    {
        builder.HasKey(item => item.Id);

        builder.Property(item => item.ProductNameSnapshot).HasMaxLength(200).IsRequired();
        builder.Property(item => item.BrandSnapshot).HasMaxLength(200);
        builder.Property(item => item.ModelSnapshot).HasMaxLength(200);
        builder.Property(item => item.SkuOrCodeSnapshot).HasMaxLength(200);
        builder.Property(item => item.Quantity).HasPrecision(18, 3).IsRequired();
        builder.Property(item => item.Unit).HasMaxLength(50).IsRequired();
        builder.Property(item => item.CustomerNotes).HasColumnType("text");

        builder.HasIndex(item => item.QuoteRequestId);
        builder.HasIndex(item => item.ProductId);

        builder.HasOne(item => item.QuoteRequest)
            .WithMany(quote => quote.Items)
            .HasForeignKey(item => item.QuoteRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(item => item.Product)
            .WithMany(product => product.QuoteItems)
            .HasForeignKey(item => item.ProductId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
