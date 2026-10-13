using ControlElectricoObrera.Api.Data.Configurations;
using ControlElectricoObrera.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ControlElectricoObrera.Api.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductImage> ProductImages => Set<ProductImage>();
    public DbSet<ProductSpecification> ProductSpecifications => Set<ProductSpecification>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<QuoteRequest> QuoteRequests => Set<QuoteRequest>();
    public DbSet<QuoteItem> QuoteItems => Set<QuoteItem>();
    public DbSet<QuoteAttachment> QuoteAttachments => Set<QuoteAttachment>();
    public DbSet<SpecialMaterialRequest> SpecialMaterialRequests => Set<SpecialMaterialRequest>();
    public DbSet<SpecialMaterialAttachment> SpecialMaterialAttachments => Set<SpecialMaterialAttachment>();
    public DbSet<AdminUser> AdminUsers => Set<AdminUser>();
    public DbSet<SiteSettings> SiteSettings => Set<SiteSettings>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new ProductImageConfiguration());
        modelBuilder.ApplyConfiguration(new ProductSpecificationConfiguration());
        modelBuilder.ApplyConfiguration(new BrandConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        modelBuilder.ApplyConfiguration(new QuoteRequestConfiguration());
        modelBuilder.ApplyConfiguration(new QuoteItemConfiguration());
        modelBuilder.ApplyConfiguration(new QuoteAttachmentConfiguration());
        modelBuilder.ApplyConfiguration(new SpecialMaterialRequestConfiguration());
        modelBuilder.ApplyConfiguration(new SpecialMaterialAttachmentConfiguration());
        modelBuilder.ApplyConfiguration(new AdminUserConfiguration());
        modelBuilder.ApplyConfiguration(new SiteSettingsConfiguration());
    }
}
