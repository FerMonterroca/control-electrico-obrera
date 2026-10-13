using ControlElectricoObrera.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlElectricoObrera.Api.Data.Configurations;

public sealed class QuoteAttachmentConfiguration : IEntityTypeConfiguration<QuoteAttachment>
{
    public void Configure(EntityTypeBuilder<QuoteAttachment> builder)
    {
        builder.HasKey(attachment => attachment.Id);

        builder.Property(attachment => attachment.OriginalFileName).HasMaxLength(255).IsRequired();
        builder.Property(attachment => attachment.StorageKey).HasMaxLength(1000).IsRequired();
        builder.Property(attachment => attachment.ContentType).HasMaxLength(200).IsRequired();
        builder.Property(attachment => attachment.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(attachment => attachment.QuoteRequestId);

        builder.HasOne(attachment => attachment.QuoteRequest)
            .WithMany(quote => quote.Attachments)
            .HasForeignKey(attachment => attachment.QuoteRequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
