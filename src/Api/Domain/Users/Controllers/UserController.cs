using Api.Domain.Users.DTOs;
using Api.Domain.Users.Models;
using Api.Domain.Users.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Domain.Users.Controllers;

[ApiController]
[Route("api/users")]
public class UserController(IUserService service, ICurrentUserService currentUserService, IMapper mapper) : ControllerBase
{
	[Authorize]
    [HttpGet]
    public async Task<ActionResult<ICollection<GetUserResponseDto>>> GetAllUsers()
    {
        var users = await service.GetAllAsync();
        return Ok(mapper.Map<ICollection<GetUserResponseDto>>(users));
    }

	[Authorize]
    [HttpGet("{userId}")]
    public async Task<ActionResult<GetUserResponseDto>> GetUser(Guid userId)
    {
        var user = await service.GetByIdAsync(userId);
        return Ok(mapper.Map<GetUserResponseDto>(user));
    }

	[Authorize]
    [HttpGet("search")]
    public async Task<ActionResult<GetUserResponseDto>> GetUser([FromQuery] string email, [FromQuery] string username)
    {
        if(string.IsNullOrEmpty(email) && string.IsNullOrEmpty(username))
            return BadRequest("Either email or username must be provided.");
        
        var user = string.IsNullOrEmpty(email) 
            ? await service.GetByUsernameAsync(username)
            : await service.GetByEmailAsync(email);
		
        return Ok(mapper.Map<GetUserResponseDto>(user));
    }

	[Authorize]
    [HttpPatch("{userId}")]
    public async Task<ActionResult<UpdateUserResponseDto>> UpdateUser(Guid userId, [FromBody] UpdateUserRequestDto dto)
    {
		if(currentUserService.GetCurrentUserId() != userId)
            return Unauthorized("You can only update your own account.");

        await service.CheckPasswordAsync(userId, dto.Password.Trim());
        var updatedUser = await service.UpdateAsync(userId, dto);
        return Ok(mapper.Map<UpdateUserResponseDto>(updatedUser));
    }

	[Authorize]
    [HttpDelete("{userId}")]
    public async Task<ActionResult> DeleteUser(Guid userId, [FromBody] DeleteUserRequestDto dto)
    {
		if(currentUserService.GetCurrentUserId() != userId)
            return Unauthorized("You can only delete your own account.");

        await service.CheckPasswordAsync(userId, dto.Password.Trim());
        await service.DeleteAsync(userId);
        return Ok();
    }
}