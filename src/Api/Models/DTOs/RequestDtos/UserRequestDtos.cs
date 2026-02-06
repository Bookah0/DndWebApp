using System.ComponentModel.DataAnnotations;

namespace Api.Models.DTOs.RequestDtos;

public class GetUserRequestDto
{
    public required string UsernameOrEmail { get; set; }
}

public class UpdateUserRequestDto
{
    public string? Username { get; set; }
    public string? Email { get; set; }

    [Required]
    [MinLength(6)]
    [MaxLength(100)]
    public required string ConfirmPassword { get; set; }
}

public class ConfirmPasswordDto
{
    public string? Username { get; set; }
    public string? Email { get; set; }

    [Required]
    [MinLength(6)]
    [MaxLength(100)]
    public required string ConfirmPassword { get; set; }
}

public class LoginUserRequestDto
{
    [MinLength(1)]
    [MaxLength(50)]
    public required string UsernameOrEmail { get; set; }
    
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

    [Required]
    [MinLength(6)]
    [MaxLength(100)]
    public required string ConfirmPassword { get; set; }
}