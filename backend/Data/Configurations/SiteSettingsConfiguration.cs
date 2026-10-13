using ControlElectricoObrera.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlElectricoObrera.Api.Data.Configurations;

public sealed class SiteSettingsConfiguration : IEntityTypeConfiguration<SiteSettings>
{
    public void Configure(EntityTypeBuilder<SiteSettings> builder)
    {
        builder.HasKey(settings => settings.Id);

        builder.Property(settings => settings.BusinessName).HasMaxLength(200).IsRequired();
        builder.Property(settings => settings.Phone).HasMaxLength(50);
        builder.Property(settings => settings.WhatsApp).HasMaxLength(50);
        builder.Property(settings => settings.Email).HasMaxLength(320);
        builder.Property(settings => settings.GeneralLocation).HasMaxLength(500);
        builder.Property(settings => settings.BusinessHours).HasColumnType("text");
        builder.Property(settings => settings.TwentyFourSevenRequestsEnabled).HasDefaultValue(false);
        builder.Property(settings => settings.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(settings => settings.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}
