using ControlElectricoObrera.Api.Data;
using ControlElectricoObrera.Api.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace ControlElectricoObrera.Api.Tests.Infrastructure;

public sealed class CatalogApiFixture : IAsyncLifetime
{
    private CatalogWebApplicationFactory? factory;
    private HttpClient? client;
    private bool serverStarted;

    public HttpClient Client => client
        ?? throw new InvalidOperationException("The test fixture has not been initialized.");

    public async Task InitializeAsync()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_CONNECTION")
            ?? throw new InvalidOperationException(
                "TEST_DATABASE_CONNECTION is required for integration tests.");

        var connectionBuilder = new NpgsqlConnectionStringBuilder(connectionString);
        if (!string.Equals(
                connectionBuilder.Database,
                "control_electrico_obrera_tests",
                StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                "Integration tests only accept the control_electrico_obrera_tests database.");
        }

        connectionBuilder.Database =
            $"control_electrico_obrera_tests_{Guid.NewGuid():N}";
        connectionString = connectionBuilder.ConnectionString;

        factory = new CatalogWebApplicationFactory(connectionString);
        client = factory.CreateClient();
        serverStarted = true;

        await using var scope = factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();
        await SeedAsync(dbContext);
    }

    public async Task DisposeAsync()
    {
        if (factory is not null && serverStarted)
        {
            await using var scope = factory.Services.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await dbContext.Database.EnsureDeletedAsync();
        }

        client?.Dispose();
        factory?.Dispose();
    }

    private static async Task SeedAsync(AppDbContext dbContext)
    {
        var schneider = new Brand
        {
            Name = "Schneider Electric",
            Slug = "schneider-electric"
        };
        var hiddenBrand = new Brand
        {
            Name = "Inactive Test Brand",
            Slug = "inactive-test-brand",
            IsActive = false
        };
        var contactors = new Category
        {
            Name = "Contactors",
            Slug = "contactors"
        };
        var protection = new Category
        {
            Name = "Electrical Protection",
            Slug = "electrical-protection"
        };
        var hiddenCategory = new Category
        {
            Name = "Inactive Test Category",
            Slug = "inactive-test-category",
            IsActive = false
        };

        dbContext.AddRange(schneider, hiddenBrand, contactors, protection, hiddenCategory);
        dbContext.Products.AddRange(
            new Product
            {
                Name = "Test Contactor 18 A",
                Slug = "test-contactor-18a",
                ShortDescription = "Development-only contactor.",
                Brand = schneider,
                Category = contactors,
                Model = "TEST-LC18",
                SkuOrCode = "DEV-CTR-018",
                Unit = "piece",
                AvailabilityStatus = ProductAvailabilityStatus.Consult,
                IsFeatured = true,
                Images =
                {
                    new ProductImage
                    {
                        StorageKey = "development/products/test-contactor.webp",
                        AltText = "Test contactor",
                        IsPrimary = true
                    }
                },
                Specifications =
                {
                    new ProductSpecification { Name = "Current", Value = "18", Unit = "A" }
                }
            },
            new Product
            {
                Name = "Test Circuit Breaker",
                Slug = "test-circuit-breaker",
                Brand = schneider,
                Category = protection,
                Model = "MODEL-QO-TEST",
                SkuOrCode = "DEV-BRK-020",
                Unit = "piece",
                AvailabilityStatus = ProductAvailabilityStatus.AvailableOnRequest
            },
            new Product
            {
                Name = "Inactive Test Product",
                Slug = "inactive-test-product",
                Brand = schneider,
                Category = contactors,
                SkuOrCode = "DEV-HIDDEN-001",
                Unit = "piece",
                IsActive = false
            });

        await dbContext.SaveChangesAsync();
    }
}

internal sealed class CatalogWebApplicationFactory(string connectionString)
    : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.UseSetting("ConnectionStrings:Default", connectionString);
        builder.UseSetting("Cors:AllowedOrigins:0", "http://localhost:4321");
    }
}
