using AutoMapper;
using Api.Models.DTOs.RequestDtos.Character;
using Api.Models.DTOs.ResponseDtos;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/abilities")]
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