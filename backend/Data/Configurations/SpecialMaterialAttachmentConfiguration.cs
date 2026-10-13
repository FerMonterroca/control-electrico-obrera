using ControlElectricoObrera.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlElectricoObrera.Api.Data.Configurations;

public sealed class SpecialMaterialAttachmentConfiguration : IEntityTypeConfiguration<SpecialMaterialAttachment>
{
    public void Configure(EntityTypeBuilder<SpecialMaterialAttachment> builder)
    {
        builder.HasKey(attachment => attachment.Id);

        builder.Property(attachment => attachment.OriginalFileName).HasMaxLength(255).IsRequired();
        builder.Property(attachment => attachment.StorageKey).HasMaxLength(1000).IsRequired();
        builder.Property(attachment => attachment.ContentType).HasMaxLength(200).IsRequired();
        builder.Property(attachment => attachment.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(attachment => attachment.SpecialMaterialRequestId);

        builder.HasOne(attachment => attachment.SpecialMaterialRequest)
            .WithMany(request => request.Attachments)
            .HasForeignKey(attachment => attachment.SpecialMaterialRequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
