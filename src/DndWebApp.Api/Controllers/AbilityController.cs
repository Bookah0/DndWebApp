using AutoMapper;
using DndWebApp.Api.Models.DTOs.Character;
using DndWebApp.Api.Models.DTOs.ResponseDtos;
using DndWebApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DndWebApp.Api.Controllers;

[ApiController]
[Route("api/[controller]s")]
public class AbilityController(IAbilityService service, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ICollection<AbilityResponseDto>>> GetAbilities()
    {
        var abilities = await service.GetAllAsync();
        return Ok(mapper.Map<ICollection<AbilityResponseDto>>(abilities));
    }

    [HttpGet("{abilityId}")]
    public async Task<ActionResult<AbilityResponseDto>> GetAbility(int abilityId)
    {
        var ability = await service.GetByIdAsync(abilityId);
        return Ok(mapper.Map<AbilityResponseDto>(ability));
    }
}