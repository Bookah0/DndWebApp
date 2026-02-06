namespace Api.Models.DTOs.RequestDtos;

public class GetUserRequestDto
{
    public string? Username { get; set; }
    public string? Email { get; set; }
}

public class UpdateUserRequestDto
{
    public string? Username { get; set; }
    public string? Email { get; set; }
}

public class RegisterUserRequestDto
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}