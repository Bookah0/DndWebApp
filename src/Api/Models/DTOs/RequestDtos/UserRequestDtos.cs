using System.ComponentModel.DataAnnotations;

namespace Api.Models.DTOs.RequestDtos;

public class GetUserRequestDto
{
    [MinLength(1)]
    [MaxLength(50)]
    public string? Username { get; set; }

    [EmailAddress]
    public string? Email { get; set; }
}

public class UpdateUserRequestDto
{
    public string? Username { get; set; }
    public string? Email { get; set; }

    [Required]
    [MinLength(6)]
    [MaxLength(100)]
    public required string Password { get; set; }
}

public class DeleteUserRequestDto
{
    public string? Username { get; set; }
    public string? Email { get; set; }

    [Required]
    [MinLength(6)]
    [MaxLength(100)]
    public required string Password { get; set; }
}

public class LoginUserRequestDto
{
    [MinLength(1)]
    [MaxLength(50)]
    public string? Username { get; set; }

    [EmailAddress]
    public string? Email { get; set; }
    
    [Required]
    [MinLength(6)]
    [MaxLength(100)]
    public required string Password { get; set; }
}

public class RegisterUserRequestDto
{
    [MinLength(1)]
    [MaxLength(50)]
    public string? Username { get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    
    [Required]
    [MinLength(6)]
    [MaxLength(100)]
    public required string Password { get; set; }
}