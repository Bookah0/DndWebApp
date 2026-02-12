namespace Api.Models.DTOs.ResponseDtos;

public class GetUserResponseDto
{
    public required Guid Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }
}

public class RegisterUserResponseDto
{
    public required Guid Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }
}

public class UpdateUserResponseDto
{
    public required Guid Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }
}