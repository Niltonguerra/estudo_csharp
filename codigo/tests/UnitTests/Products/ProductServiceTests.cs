using FluentAssertions;
using Moq;
using ProductsApi.Modules.Products.Application.DTOs;
using ProductsApi.Modules.Products.Application.Services;
using ProductsApi.Modules.Products.Domain.Entities;
using ProductsApi.Modules.Products.Domain.Interfaces;
using ProductsApi.Modules.Products.Infrastructure.Messaging;

namespace UnitTests.Products;

public class ProductServiceTests
{
    // Arrange global — compartilhado entre todos os testes
    private readonly Mock<IProductRepository> _repositoryMock;
    private readonly Mock<ProductEventPublisher> _publisherMock;
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _repositoryMock = new Mock<IProductRepository>();
        _publisherMock = new Mock<IProductEventPublisher>();
        _service = new ProductService(_repositoryMock.Object, _publisherMock.Object);
    }

    // =====================
    // GetAllAsync
    // =====================

    [Fact]
    public async Task GetAllAsync_DeveRetornarListaDeProdutos()
    {
        // Arrange
        var products = new List<Product>
        {
            new() { Id = 1, Name = "Produto 1", Description = "Desc 1", Price = 10, Stock = 5 },
            new() { Id = 2, Name = "Produto 2", Description = "Desc 2", Price = 20, Stock = 10 }
        };

        _repositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(products);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.First().Name.Should().Be("Produto 1");
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarListaVazia_QuandoNaoHaProdutos()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Product>());

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().BeEmpty();
    }

    // =====================
    // GetByIdAsync
    // =====================

    [Fact]
    public async Task GetByIdAsync_DeveRetornarProduto_QuandoExiste()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Produto 1", Description = "Desc 1", Price = 10, Stock = 5 };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(product);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Name.Should().Be("Produto 1");
    }

    [Fact]
    public async Task GetByIdAsync_DeveRetornarNull_QuandoNaoExiste()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.GetByIdAsync(99))
            .ReturnsAsync((Product?)null);

        // Act
        var result = await _service.GetByIdAsync(99);

        // Assert
        result.Should().BeNull();
    }

    // =====================
    // CreateAsync
    // =====================

    [Fact]
    public async Task CreateAsync_DeveCriarProduto_ERetornarResponse()
    {
        // Arrange
        var dto = new CreateProductDto("Novo Produto", "Descrição", 29.99m, 100);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Novo Produto");
        result.Price.Should().Be(29.99m);

        // Verifica se o repositório foi chamado corretamente
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
        _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    // =====================
    // UpdateAsync
    // =====================

    [Fact]
    public async Task UpdateAsync_DeveAtualizarProduto_QuandoExiste()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Antigo", Description = "Antiga", Price = 10, Stock = 5 };
        var dto = new UpdateProductDto("Novo Nome", "Nova Desc", 99.99m, 50);

        _repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(product);

        // Act
        var result = await _service.UpdateAsync(1, dto);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Novo Nome");
        result.Price.Should().Be(99.99m);

        _repositoryMock.Verify(r => r.Update(It.IsAny<Product>()), Times.Once);
        _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_DeveRetornarNull_QuandoProdutoNaoExiste()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.GetByIdAsync(99))
            .ReturnsAsync((Product?)null);

        var dto = new UpdateProductDto("Nome", "Desc", 10, 5);

        // Act
        var result = await _service.UpdateAsync(99, dto);

        // Assert
        result.Should().BeNull();
        _repositoryMock.Verify(r => r.Update(It.IsAny<Product>()), Times.Never);
        _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    // =====================
    // DeleteAsync
    // =====================

    [Fact]
    public async Task DeleteAsync_DeveRetornarTrue_QuandoProdutoExiste()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Produto", Description = "Desc", Price = 10, Stock = 5 };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(product);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        result.Should().BeTrue();
        _repositoryMock.Verify(r => r.Remove(It.IsAny<Product>()), Times.Once);
        _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_DeveRetornarFalse_QuandoProdutoNaoExiste()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.GetByIdAsync(99))
            .ReturnsAsync((Product?)null);

        // Act
        var result = await _service.DeleteAsync(99);

        // Assert
        result.Should().BeFalse();
        _repositoryMock.Verify(r => r.Remove(It.IsAny<Product>()), Times.Never);
        _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}