using ControlElectricoObrera.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlElectricoObrera.Api.Data.Configurations;

public sealed class QuoteRequestConfiguration : IEntityTypeConfiguration<QuoteRequest>
{
    public void Configure(EntityTypeBuilder<QuoteRequest> builder)
    {
        builder.HasKey(quote => quote.Id);

        builder.Property(quote => quote.Folio).HasMaxLength(100).IsRequired();
        builder.Property(quote => quote.CustomerNameSnapshot).HasMaxLength(200).IsRequired();
        builder.Property(quote => quote.CustomerPhoneSnapshot).HasMaxLength(50).IsRequired();
        builder.Property(quote => quote.CustomerEmailSnapshot).HasMaxLength(320);
        builder.Property(quote => quote.CompanyNameSnapshot).HasMaxLength(200);
        builder.Property(quote => quote.Status).HasConversion<int>().HasDefaultValue(QuoteStatus.New);
        builder.Property(quote => quote.PostalCode).HasMaxLength(20);
        builder.Property(quote => quote.NeededByDate).HasColumnType("date");
        builder.Property(quote => quote.Comments).HasColumnType("text");
        builder.Property(quote => quote.Source).HasConversion<int>().IsRequired();
        builder.Property(quote => quote.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(quote => quote.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(quote => quote.Folio).IsUnique();
        builder.HasIndex(quote => quote.CustomerId);
        builder.HasIndex(quote => quote.Status);
        builder.HasIndex(quote => quote.CreatedAt);

        builder.HasOne(quote => quote.Customer)
            .WithMany(customer => customer.QuoteRequests)
            .HasForeignKey(quote => quote.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
