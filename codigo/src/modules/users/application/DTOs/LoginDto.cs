using System.ComponentModel.DataAnnotations;

namespace ProductsApi.Modules.Users.Application.DTOs;

public record LoginDto(
    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    string Email,

    [Required(ErrorMessage = "Senha é obrigatória")]
    string Password
);