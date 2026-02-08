using AutoMapper;
using Api.Models.DTOs.ResponseDtos;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Api.Models.DTOs.RequestDtos;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers.Users;

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