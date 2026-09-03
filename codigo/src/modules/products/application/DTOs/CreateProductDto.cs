using System.ComponentModel.DataAnnotations;

namespace ProductsApi.Modules.Products.Application.DTOs;

public record CreateProductDto(
    [Required(ErrorMessage = "Nome é obrigatório")]
    [MaxLength(200, ErrorMessage = "Nome deve ter no máximo 200 caracteres")]
    string Name,

    [Required(ErrorMessage = "Descrição é obrigatória")]
    [MaxLength(1000, ErrorMessage = "Descrição deve ter no máximo 1000 caracteres")]
    string Description,

    [Range(0.01, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero")]
    decimal Price,

    [Required(ErrorMessage = "Estoque é obrigatório")]
    [Range(0, int.MaxValue, ErrorMessage = "Estoque não pode ser negativo")]
    int Stock
);