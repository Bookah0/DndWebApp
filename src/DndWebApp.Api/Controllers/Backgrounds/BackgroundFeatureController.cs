using System.ComponentModel.DataAnnotations;
using AutoMapper;
using DndWebApp.Api.Models.DTOs.RequestDtos.Character;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.DTOs.ResponseDtos;
using DndWebApp.Api.Services.Interfaces;
using DndWebApp.Api.Services.Interfaces.Features;
using Microsoft.AspNetCore.Mvc;
using DndWebApp.Api.Models.Features;

namespace DndWebApp.Api.Controllers.Features;

[ApiController]
[Route("api/backgrounds/{backgroundId}/features")]
public class BackgroundFeatureController(IFeatureService<BackgroundFeature, BackgroundFeatureDto> service, IBackgroundService backgroundService, IMapper mapper) : ControllerBase
{   
    [HttpGet]
    public async Task<ActionResult<ICollection<BackgroundFeatureResponseDto>>> GetBackgroundFeatures(int backgroundId)
    {
        var background = await backgroundService.GetWithFeaturesAsync(backgroundId);
        var features = background.Features;
        return Ok(mapper.Map<ICollection<BackgroundFeatureResponseDto>>(features));
    }

    [HttpGet("{featureId}")]
    public async Task<ActionResult<BackgroundFeatureResponseDto>> GetBackgroundFeature(int featureId, int backgroundId)
    {
        var backgroundFeature = await service.GetByIdAsync(featureId);

        if(backgroundFeature.BackgroundId != backgroundId)
            throw new ValidationException($"Background feature with id {backgroundFeature.Id} does not belong to background with id {backgroundId}");

        return Ok(mapper.Map<BackgroundFeatureResponseDto>(backgroundFeature));
    }

    [HttpPost]
    public async Task<ActionResult<BackgroundFeatureResponseDto>> CreateBackgroundFeature([FromBody] BackgroundFeatureDto dto, int backgroundId)
    {
        if(dto.BackgroundId != backgroundId)
            throw new ValidationException($"Background id in dto {dto.BackgroundId} does not match background id in route {backgroundId}");

        var backgroundFeature = await service.CreateAsync(dto);
        return Ok(mapper.Map<BackgroundFeatureResponseDto>(backgroundFeature));
    }

    [HttpPatch("{featureId}")]
    public async Task<ActionResult> UpdateBackgroundFeature(int featureId, int backgroundId, [FromBody] BackgroundFeatureDto dto)
    {
        await EnsureFeatureBelongsToBackground(backgroundId, featureId);
        await service.UpdateAsync(dto, featureId);
        return Ok();
    }

    [HttpDelete("{featureId}")]
    public async Task<ActionResult> DeleteBackgroundFeature(int featureId, int backgroundId)
    {
        await EnsureFeatureBelongsToBackground(backgroundId, featureId);
        await service.DeleteAsync(featureId);
        return Ok();
    }

    // Spell management endpoints
    [HttpPost("{featureId}/spells/{spellId}")]
    public async Task<ActionResult> AddSpell(int featureId, int spellId, int backgroundId)
    {
        await EnsureFeatureBelongsToBackground(backgroundId, featureId);
        await service.AddSpell(spellId, featureId);
        return Ok();
    }
    
    [HttpDelete("{featureId}/spells/{spellId}")]
    public async Task<ActionResult> RemoveSpell(int featureId, int spellId, int backgroundId)
    {
        await EnsureFeatureBelongsToBackground(backgroundId, featureId);
        await service.RemoveSpell(spellId, featureId);
        return Ok();
    }

    // Proficiency management endpoints
    [HttpPost("{featureId}/proficiencies")]
    public async Task<ActionResult> AddProficiency(int featureId, int backgroundId, [FromBody] ProficiencyDto proficiency)
    {
        await EnsureFeatureBelongsToBackground(backgroundId, featureId);
        await service.AddProficiency(proficiency, featureId);
        return Ok();
    }

    [HttpDelete("{featureId}/proficiencies")]
    public async Task<ActionResult> RemoveProficiency(int featureId, int backgroundId, [FromBody] ProficiencyDto proficiency)
    {
        await EnsureFeatureBelongsToBackground(backgroundId, featureId);
        await service.RemoveProficiency(proficiency, featureId);
        return Ok();
    }

    // Ability increase management endpoints
    [HttpPost("{featureId}/ability-increases")]
    public async Task<ActionResult> AddAbilityIncrease(int featureId, int backgroundId, [FromBody] AbilityValueDto increase)
    {
        await EnsureFeatureBelongsToBackground(backgroundId, featureId);
        await service.AddAbilityIncrease(increase.AbilityId, increase.Value, featureId);
        return Ok();
    }

    [HttpDelete("{featureId}/ability-increases")]
    public async Task<ActionResult> RemoveAbilityIncrease(int featureId, int backgroundId, [FromBody] AbilityValueDto increase)
    {
        await EnsureFeatureBelongsToBackground(backgroundId, featureId);
        await service.RemoveAbilityIncrease(increase.AbilityId, featureId);
        return Ok();
    }

    // Helpers
    private async Task EnsureFeatureBelongsToBackground(int backgroundId, int featureId)
    {
        var feature = await service.GetByIdAsync(featureId);

        if (feature.BackgroundId != backgroundId)
            throw new ValidationException($"Background feature with id {feature.Id} does not belong to background with id {backgroundId}");
    }
}

