using ControlElectricoObrera.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlElectricoObrera.Api.Data.Configurations;

public sealed class SpecialMaterialRequestConfiguration : IEntityTypeConfiguration<SpecialMaterialRequest>
{
    public void Configure(EntityTypeBuilder<SpecialMaterialRequest> builder)
    {
        builder.HasKey(request => request.Id);

        builder.Property(request => request.CustomerNameSnapshot).HasMaxLength(200).IsRequired();
        builder.Property(request => request.CustomerPhoneSnapshot).HasMaxLength(50).IsRequired();
        builder.Property(request => request.CustomerEmailSnapshot).HasMaxLength(320);
        builder.Property(request => request.CompanyNameSnapshot).HasMaxLength(200);
        builder.Property(request => request.ProductName).HasMaxLength(200);
        builder.Property(request => request.Brand).HasMaxLength(200);
        builder.Property(request => request.Model).HasMaxLength(200);
        builder.Property(request => request.Code).HasMaxLength(200);
        builder.Property(request => request.Quantity).HasPrecision(18, 3);
        builder.Property(request => request.Unit).HasMaxLength(50).IsRequired();
        builder.Property(request => request.Description).HasColumnType("text").IsRequired();
        builder.Property(request => request.Urgency).HasMaxLength(50).IsRequired();
        builder.Property(request => request.Status)
            .HasConversion<int>()
            .HasDefaultValue(SpecialMaterialRequestStatus.New);
        builder.Property(request => request.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(request => request.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(request => request.CustomerId);
        builder.HasIndex(request => request.Status);

        builder.HasOne(request => request.Customer)
            .WithMany(customer => customer.SpecialMaterialRequests)
            .HasForeignKey(request => request.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
