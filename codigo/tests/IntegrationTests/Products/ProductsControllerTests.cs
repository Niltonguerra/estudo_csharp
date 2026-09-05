using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using IntegrationTests.Setup;
using ProductsApi.Modules.Products.Application.DTOs;

namespace IntegrationTests.Products;

public class ProductsControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ProductsControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_DeveRetornar200_QuandoNaoHaProdutos()
    {
        // Act
        var response = await _client.GetAsync("/products");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Create_DeveRetornar403_QuandoNaoAutenticado()
    {
        // Arrange
        var dto = new CreateProductDto("Produto Teste", "Descrição", 29.99m, 100);

        // Act
        var response = await _client.PostAsJsonAsync("/products", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}