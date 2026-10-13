using System.Net;
using System.Text.Json;
using ControlElectricoObrera.Api.Tests.Infrastructure;

namespace ControlElectricoObrera.Api.Tests;

public sealed class CatalogApiTests(CatalogApiFixture fixture)
    : IClassFixture<CatalogApiFixture>
{
    private readonly HttpClient client = fixture.Client;

    [Fact]
    public async Task Products_ReturnsOk()
    {
        var response = await client.GetAsync("/api/products");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Products_WithInvalidPage_ReturnsBadRequest()
    {
        await AssertErrorAsync("/api/products?page=0", "invalid_page");
    }

    [Fact]
    public async Task Products_WithExcessivePageSize_ReturnsBadRequest()
    {
        await AssertErrorAsync("/api/products?pageSize=10000", "invalid_page_size");
    }

    [Fact]
    public async Task Products_WithInvalidAvailability_ReturnsBadRequest()
    {
        await AssertErrorAsync("/api/products?availability=Unknown", "invalid_availability");
    }

    [Fact]
    public async Task Products_WithInvalidSort_ReturnsBadRequest()
    {
        await AssertErrorAsync("/api/products?sort=drop-table", "invalid_sort");
    }

    [Fact]
    public async Task Products_WithInvalidFeatured_ReturnsBadRequest()
    {
        await AssertErrorAsync("/api/products?featured=perhaps", "invalid_featured");
    }

    [Fact]
    public async Task MissingProduct_ReturnsNotFound()
    {
        var response = await client.GetAsync("/api/products/does-not-exist");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task InactiveProduct_IsNotPublic()
    {
        var response = await client.GetAsync("/api/products/inactive-test-product");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task InactiveBrand_IsNotListed()
    {
        using var document = await GetJsonAsync("/api/brands");
        var slugs = document.RootElement.EnumerateArray()
            .Select(item => item.GetProperty("slug").GetString());

        Assert.DoesNotContain("inactive-test-brand", slugs);
    }

    [Fact]
    public async Task InactiveCategory_IsNotListed()
    {
        using var document = await GetJsonAsync("/api/categories");
        var slugs = document.RootElement.EnumerateArray()
            .Select(item => item.GetProperty("slug").GetString());

        Assert.DoesNotContain("inactive-test-category", slugs);
    }

    [Theory]
    [InlineData("Contactor", "test-contactor-18a")]
    [InlineData("DEV-CTR-018", "test-contactor-18a")]
    [InlineData("MODEL-QO-TEST", "test-circuit-breaker")]
    [InlineData("Schneider Electric", "test-contactor-18a")]
    public async Task Search_FindsProductsBySupportedFields(string search, string expectedSlug)
    {
        using var document = await GetJsonAsync($"/api/products?search={Uri.EscapeDataString(search)}");
        var slugs = document.RootElement.GetProperty("items").EnumerateArray()
            .Select(item => item.GetProperty("slug").GetString());

        Assert.Contains(expectedSlug, slugs);
    }

    private async Task AssertErrorAsync(string path, string expectedCode)
    {
        var response = await client.GetAsync(path);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal(expectedCode, document.RootElement.GetProperty("error").GetProperty("code").GetString());
    }

    private async Task<JsonDocument> GetJsonAsync(string path)
    {
        var response = await client.GetAsync(path);
        response.EnsureSuccessStatusCode();
        return JsonDocument.Parse(await response.Content.ReadAsStringAsync());
    }
}
