using System.IdentityModel.Tokens.Jwt;

namespace Api.Domain.Users.DTOs;

public class GetUserResponseDto
{
    public required Guid Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }
}

public class LoginUserResponseDto
{
    public required JwtSecurityToken Token { get; set; }
	public required string Expiration { get; set; }
}

public class UpdateUserResponseDto
{
    public required Guid Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }
}