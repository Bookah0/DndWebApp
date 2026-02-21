using Api.Domain.Abilities.DTOs;
using Api.Domain.Abilities.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Api.Domain.Abilities.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AbilitiesController(IAbilityService service, IMapper mapper) : ControllerBase
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