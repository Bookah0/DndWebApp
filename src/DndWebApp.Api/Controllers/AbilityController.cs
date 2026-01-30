using DndWebApp.Api.Models.DTOs.Character;
using DndWebApp.Api.Models.DTOs.ResponseDtos;
using DndWebApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DndWebApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AbilityController : ControllerBase
{
    public IAbilityService service;

    public AbilityController(IAbilityService service)
    {
        this.service = service;
    }
    
    [HttpGet]
    public async Task<ActionResult<ICollection<AbilityResponseDto>>> GetAbilities()
    {
        var abilities = await service.GetAllAsync();
        return Ok(abilities);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AbilityResponseDto>> GetAbility(int id)
    {
        var ability = await service.GetByIdAsync(id);
        return Ok(ability);
    }
}