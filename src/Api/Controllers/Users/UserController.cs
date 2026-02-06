using AutoMapper;
using Api.Models.DTOs.RequestDtos;
using Api.Models.DTOs.ResponseDtos;
using Microsoft.AspNetCore.Mvc;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Api.Models.Users;

namespace Api.Controllers.Users;

[ApiController]
[Route("api/users")]
public class UserController(IUserService service, IMapper mapper) : ControllerBase
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

    [HttpGet]
    public async Task<ActionResult<GetUserResponseDto>> GetUser([FromQuery] string email, [FromQuery] string username)
    {
        if(string.IsNullOrEmpty(email) && string.IsNullOrEmpty(username))
            return BadRequest("Either email or username must be provided.");
        
        User user = string.IsNullOrEmpty(email) 
            ? await service.GetByUsernameAsync(username)
            : await service.GetByEmailAsync(email);

        return Ok(mapper.Map<GetUserResponseDto>(user));
    }

    [Authorize]
    [HttpPatch("{userId}")]
    public async Task<ActionResult<UpdateUserResponseDto>> UpdateUser(Guid userId, [FromBody] UpdateUserRequestDto dto)
    {
        await service.CheckPasswordAsync(userId, dto.ConfirmPassword);
        var updatedUser = await service.UpdateAsync(userId, dto);
        return Ok(mapper.Map<UpdateUserResponseDto>(updatedUser));
    }

    [Authorize]
    [HttpDelete("{userId}")]
    public async Task<ActionResult> DeleteUser(Guid userId, [FromBody] ConfirmPasswordDto dto)
    {
        await service.CheckPasswordAsync(userId, dto.ConfirmPassword);
        await service.DeleteAsync(userId);
        return Ok();
    }
}