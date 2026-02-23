using Api.Domain.Users.DTOs;
using Api.Domain.Users.Models;
using Api.Domain.Users.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Api.Domain.Users.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserContentController(IUserService service, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ICollection<GetUserResponseDto>>> GetAllUsers([FromQuery] string? sort = null, [FromQuery] string? order = null)
    {
        var users = await service.GetAllAsync();
        return Ok(mapper.Map<ICollection<GetUserResponseDto>>(users));
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<GetUserResponseDto>> GetUser(Guid userId)
    {
        var user = await service.GetByIdAsync(userId);
        return Ok(mapper.Map<GetUserResponseDto>(user));
    }

    [HttpGet("search")]
    public async Task<ActionResult<GetUserResponseDto>> GetUser([FromQuery] string email, [FromQuery] string username)
    {
        if(string.IsNullOrEmpty(email) && string.IsNullOrEmpty(username))
            return BadRequest("Either email or username must be provided.");
        
        User user = string.IsNullOrEmpty(email) 
            ? await service.GetByUsernameAsync(username)
            : await service.GetByEmailAsync(email);

        return Ok(mapper.Map<GetUserResponseDto>(user));
    }

    //[Authorize]
    [HttpPatch("{userId}")]
    public async Task<ActionResult<UpdateUserResponseDto>> UpdateUser(Guid userId, [FromBody] UpdateUserRequestDto dto)
    {
        await service.CheckPasswordAsync(userId, dto.Password.Trim());
        var updatedUser = await service.UpdateAsync(userId, dto);
        return Ok(mapper.Map<UpdateUserResponseDto>(updatedUser));
    }

    //[Authorize]
    [HttpDelete("{userId}")]
    public async Task<ActionResult> DeleteUser(Guid userId, [FromBody] DeleteUserRequestDto dto)
    {
        //await service.CheckPasswordAsync(userId, dto.Password.Trim());
        await service.DeleteAsync(userId);
        return Ok();
    }
}