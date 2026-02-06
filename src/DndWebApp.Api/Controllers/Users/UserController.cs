using AutoMapper;
using Api.Models.DTOs.Inventory;
using Api.Models.DTOs.RequestDtos;
using Api.Models.DTOs.ResponseDtos;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces.Items;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Users;

[ApiController]
[Route("api/users")]
public class UserController(IUserRepository repo, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ICollection<GetUserResponseDto>>> GetAllUsers([FromQuery] string? sort = null, [FromQuery] string? order = null)
    {
        var users = await repo.GetAllAsync();
        return Ok(mapper.Map<ICollection<GetUserResponseDto>>(users));
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<GetUserResponseDto>> GetUser(Guid userId)
    {
        var user = await repo.GetByIdAsync(userId);
        return Ok(mapper.Map<GetUserResponseDto>(user));
    }

    [HttpGet]
    public async Task<ActionResult<GetUserResponseDto>> GetUser([FromBody] GetUserRequestDto dto)
    {
        var user = dto.Username is not null 
            ? await repo.GetByUsernameAsync(dto.Username) 
            : dto.Email is not null 
                ? await repo.GetByEmailAsync(dto.Email) 
                : throw new ArgumentException("Either username or email must be provided.");

        return Ok(mapper.Map<GetUserResponseDto>(user));
    }

    [HttpPost]
    public async Task<ActionResult<RegisterUserResponseDto>> CreateUser([FromBody] RegisterUserRequestDto dto)
    {
        var user = await repo.CreateAsync(dto);
        return Ok(mapper.Map<RegisterUserResponseDto>(user));
    }

    [HttpPatch("{userId}")]
    public async Task<ActionResult<UpdateUserResponseDto>> UpdateUser(Guid userId, [FromBody] UpdateUserRequestDto dto)
    {
        var updatedUser = await repo.UpdateAsync(userId, dto);
        return Ok(mapper.Map<UpdateUserResponseDto>(updatedUser));
    }


    [HttpDelete("{userId}")]
    public async Task<ActionResult> DeleteUser(Guid userId)
    {
        await repo.DeleteAsync(userId);
        return Ok();
    }
}