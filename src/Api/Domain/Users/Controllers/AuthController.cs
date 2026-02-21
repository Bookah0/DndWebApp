using Api.Domain.Users.DTOs;
using Api.Domain.Users.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Api.Domain.Users.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AuthController(IUserService userService, ICurrentUserService currentUserService, IMapper mapper) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterUserRequestDto request)
    {
        var user = await userService.CreateAsync(request);
        await currentUserService.SetCurrentUser(user);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginUserRequestDto request)
    {
        var user = await userService.CheckPasswordAsync(request);
        await currentUserService.SetCurrentUser(user);
        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<GetUserResponseDto>> GetCurrentUser()
    {
        var currentUser = await currentUserService.GetCurrentUserAsync();
        return Ok(mapper.Map<GetUserResponseDto>(currentUser));
    }

    //[Authorize]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        await currentUserService.SetCurrentUser(null);
        return Ok();
    }
}