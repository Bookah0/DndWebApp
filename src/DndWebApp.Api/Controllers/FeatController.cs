using AutoMapper;
using DndWebApp.Api.Models.DTOs.RequestDtos.Character;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.DTOs.ResponseDtos;
using DndWebApp.Api.Services.Interfaces.Features;
using Microsoft.AspNetCore.Mvc;

namespace DndWebApp.Api.Controllers;

[ApiController]
[Route("api/[controller]s")]
public class FeatController(IFeatService service, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ICollection<FeatResponseDto>>> GetFeats()
    {
        var feats = await service.GetAllAsync();
        return Ok(mapper.Map<ICollection<FeatResponseDto>>(feats));
    }

    [HttpGet("{featId}")]
    public async Task<ActionResult<FeatResponseDto>> GetFeat(int featId)
    {
        var feat = await service.GetByIdAsync(featId);
        return Ok(mapper.Map<FeatResponseDto>(feat));
    }

    [HttpPost]
    public async Task<ActionResult<FeatResponseDto>> CreateFeat([FromBody] FeatDto dto)
    {
        var feat = await service.CreateAsync(dto);
        return Ok(mapper.Map<FeatResponseDto>(feat));
    }

    [HttpPatch("{featId}")]
    public async Task<ActionResult> UpdateFeat(int featId, [FromBody] FeatDto dto)
    {
        await service.UpdateAsync(featId, dto);
        return Ok();
    }

    [HttpDelete("{featId}")]
    public async Task<ActionResult> DeleteFeat(int featId)
    {
        await service.DeleteAsync(featId);
        return Ok();
    }

    // Spell management endpoints
    [HttpPost("{featId}/spells/{spellId}")]
    public async Task<ActionResult> AddSpell(int featId, int spellId)
    {
        await service.AddSpell(spellId, featId);
        return Ok();
    }
    
    [HttpDelete("{featId}/spells/{spellId}")]
    public async Task<ActionResult> RemoveSpell(int featId, int spellId)
    {
        await service.RemoveSpell(spellId, featId);
        return Ok();
    }

    // Proficiency management endpoints
    [HttpPost("{featId}/proficiencies")]
    public async Task<ActionResult> AddProficiency(int featId, [FromBody] ProficiencyDto proficiency)
    {
        await service.AddProficiency(proficiency, featId);
        return Ok();
    }

    [HttpDelete("{featId}/proficiencies")]
    public async Task<ActionResult> RemoveProficiency(int featId, [FromBody] ProficiencyDto proficiency)
    {
        await service.RemoveProficiency(proficiency, featId);
        return Ok();
    }

    [HttpPost("{featId}/proficiency-choices")]
    public async Task<ActionResult> AddProficiencyChoice(int featId, [FromBody] AChoiceDto dto)
    {
        await service.AddProficiencyChoice(dto, featId);
        return Ok();
    }

    [HttpDelete("{featId}/proficiency-choices/{type}/{choiceId}")]
    public async Task<ActionResult> RemoveProficiencyChoice(int featId, string type, int choiceId)
    {
        await service.RemoveProficiencyChoice(type, choiceId, featId);
        return Ok();
    }

    // Ability increase management endpoints
    [HttpPost("{featId}/ability-increases")]
    public async Task<ActionResult> AddAbilityIncrease(int featId, [FromBody] AbilityValueDto increase)
    {
        await service.AddAbilityIncrease(increase.AbilityId, increase.Value, featId);
        return Ok();
    }

    [HttpDelete("{featId}/ability-increases")]
    public async Task<ActionResult> RemoveAbilityIncrease(int featId, [FromBody] AbilityValueDto increase)
    {
        await service.RemoveAbilityIncrease(increase.AbilityId, featId);
        return Ok();
    }

    [HttpPost("{featId}/ability-increase-choices")]
    public async Task<ActionResult> AddAbilityIncreaseChoice(int featId, [FromBody] AbilityIncreaseChoiceDto increaseChoice)
    {
        await service.AddAbilityIncreaseChoice(increaseChoice, featId);
        return Ok();
    }

    [HttpDelete("{featId}/ability-increase-choices/{choiceId}")]
    public async Task<ActionResult> RemoveAbilityIncreaseChoice(int featId, int choiceId)
    {
        await service.RemoveProficiencyChoice("Ability increase", choiceId, featId);
        return Ok();
    }
}