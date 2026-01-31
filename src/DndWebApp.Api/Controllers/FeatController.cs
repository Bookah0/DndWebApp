using DndWebApp.Api.Models.DTOs.Character;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.DTOs.ResponseDtos;
using DndWebApp.Api.Services.Interfaces;
using DndWebApp.Api.Services.Interfaces.Features;
using Microsoft.AspNetCore.Mvc;

namespace DndWebApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeatController : ControllerBase
    {
    public IFeatService service;

    public FeatController(IFeatService service)
    {
        this.service = service;
    }
    
    [HttpGet]
    public async Task<ActionResult<ICollection<FeatResponseDto>>> GetFeats()
    {
        var feats = await service.GetAllAsync();
        return Ok(feats);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FeatResponseDto>> GetFeat(int id)
    {
        var feat = await service.GetByIdAsync(id);
        return Ok(feat);
    }

    [HttpPost]
    public async Task<ActionResult<FeatResponseDto>> CreateFeat([FromBody] FeatDto dto)
    {
        var feat = await service.CreateAsync(dto);
        return Ok(feat);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> UpdateFeat(int id, [FromBody] FeatDto dto)
    {
        await service.UpdateAsync(id, dto);
        return Ok();
    }

    [HttpPatch("{id}/proficiencies/{type}")]
    public async Task<ActionResult> UpdateFeat(int id, string type, [FromBody] FeatDto dto)
    {
        await service.UpdateAsync(id, dto);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteFeat(int id)
    {
        await service.DeleteAsync(id);
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
    public async Task<ActionResult> AddProficiency(int featId, [FromBody] string proficiency)
    {
        await service.AddProficiency(proficiency, featId);
        return Ok();
    }

    [HttpDelete("{featId}/proficiencies")]
    public async Task<ActionResult> RemoveProficiency(int featId, [FromBody] string proficiency)
    {
        await service.RemoveProficiency(proficiency, featId);
        return Ok();
    }
}

/*
    Task AddProficiency<TEnum>(TEnum proficiency, int featureId) where TEnum : struct, Enum;
    Task RemoveProficiency<TEnum>(TEnum proficiency, int featureId) where TEnum : struct, Enum;
    Task AddDamageAffinity(AffinityType affinityType, DamageType damageType, int featureId);
    Task RemoveDamageAffinity(AffinityType affinityType, DamageType damageType, int featureId);
    Task AddAbilityIncrease(int abilityId, int value, int featureId);
    Task RemoveAbilityIncrease(int abilityId, int featureId);
    Task AddAbilityIncreaseChoice(List<AbilityValue> options, string description, int featureId);
    Task ClearAbilityIncreaseOptions(int featureId);
    Task AddProficiencyChoice<TEnum>(List<TEnum> options, string description, int featureId) where TEnum : struct, Enum;
    Task RemoveProficiencyChoice<TEnum>(int choiceId, int featureId) where TEnum : struct, Enum;
*/