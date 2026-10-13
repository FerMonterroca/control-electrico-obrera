using ControlElectricoObrera.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlElectricoObrera.Api.Data.Configurations;

public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(customer => customer.Id);

        builder.Property(customer => customer.Name).HasMaxLength(200).IsRequired();
        builder.Property(customer => customer.Phone).HasMaxLength(50).IsRequired();
        builder.Property(customer => customer.Email).HasMaxLength(320);
        builder.Property(customer => customer.CompanyName).HasMaxLength(200);
        builder.Property(customer => customer.Notes).HasColumnType("text");
        builder.Property(customer => customer.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(customer => customer.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}
