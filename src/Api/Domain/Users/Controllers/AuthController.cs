using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api.Domain.Users.DTOs;
using Api.Domain.Users.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Api.Domain.Users.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IUserService userService, ICurrentUserService currentUserService, IConfiguration config) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<LoginUserResponseDto>> Register([FromBody] RegisterUserRequestDto request)
    {
        var user = await userService.CreateAsync(request);
        await currentUserService.SetCurrentUser(user);

		var authClaims = new List<Claim>
		{
			new(ClaimTypes.Name, user.Email ?? user.UserName!),
			new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
		};

		var token = GetToken(authClaims);
		var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
		
		return Ok(new { token = tokenString, expiration = token.ValidTo });
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginUserResponseDto>> Login([FromBody] LoginUserRequestDto request)
    {
		var user = await userService.CheckPasswordAsync(request);
		
		if (user is not null)
		{
			var authClaims = new List<Claim>
			{
				new(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new(ClaimTypes.Name, user.Email ?? user.UserName!),
				new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			var token = GetToken(authClaims);
			var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

			return Ok(new { token = tokenString, expiration = token.ValidTo });
		}
		return Unauthorized();
    }

	[Authorize]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        await currentUserService.SetCurrentUser(null);
        return Ok();
    }

	private JwtSecurityToken GetToken(List<Claim> authClaims)
	{
		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		return new JwtSecurityToken(
			issuer: config["Jwt:Issuer"],
			audience: config["Jwt:Issuer"],
			expires: DateTime.Now.AddHours(3),
			claims: authClaims,
			signingCredentials: creds
		);
	}
}