using System.ComponentModel.DataAnnotations;
using Api.Domain.Abilities.DTOs;
using Api.Domain.Backgrounds.Models;
using Api.Domain.Backgrounds.Services;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Services;
using Api.Domain.Shared.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Api.Domain.Backgrounds.Controllers;

[ApiController]
[Route("api/backgrounds/{backgroundId}/features")]
public class BackgroundFeatureController(
	IFeatureService<BackgroundFeature, CreateBackgroundFeatureRequestDto, UpdateBackgroundFeatureRequestDto, BackgroundFeatureFilterDto> service, 
	IMapper mapper
	) : ControllerBase
{   
    [HttpGet]
    public async Task<ActionResult<ICollection<BackgroundFeatureResponseDto>>> GetBackgroundFeatures(int backgroundId, [FromQuery] BackgroundFeatureFilterDto? filterDto = null, [FromQuery] PaginationRequestDto? paginationDto = null)
    {
		filterDto ??= new();
		filterDto.Background = backgroundId;

		var filteredFeatures = await service.GetAllAsync(filterDto, paginationDto);
        var mappedFeatures = mapper.Map<ICollection<BackgroundFeatureResponseDto>>(filteredFeatures);

        return Ok(PaginationUtil.BuildPaginationResponse(mappedFeatures, paginationDto, $"api/backgrounds/{backgroundId}/features"));
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
    public async Task<ActionResult<BackgroundFeatureResponseDto>> CreateBackgroundFeature([FromBody] CreateBackgroundFeatureRequestDto dto, int backgroundId)
    {
        if(dto.BackgroundId != backgroundId)
            throw new ValidationException($"Background id in dto {dto.BackgroundId} does not match background id in route {backgroundId}");

        var backgroundFeature = await service.CreateAsync(dto);
        return Ok(mapper.Map<BackgroundFeatureResponseDto>(backgroundFeature));
    }

    [HttpPatch("{featureId}")]
    public async Task<ActionResult<BackgroundFeatureResponseDto>> UpdateBackgroundFeature(int featureId, int backgroundId, [FromBody] UpdateBackgroundFeatureRequestDto dto)
    {
        await EnsureFeatureBelongsToBackground(backgroundId, featureId);
        var updatedFeature = await service.UpdateAsync(dto, featureId);
        return Ok(mapper.Map<BackgroundFeatureResponseDto>(updatedFeature));
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
    public async Task<ActionResult<BackgroundFeatureResponseDto>> AddSpell(int featureId, int spellId, int backgroundId)
    {
        await EnsureFeatureBelongsToBackground(backgroundId, featureId);
        var updatedFeature = await service.AddSpell(spellId, featureId);
        return Ok(mapper.Map<BackgroundFeatureResponseDto>(updatedFeature));
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
    public async Task<ActionResult<BackgroundFeatureResponseDto>> AddProficiency(int featureId, int backgroundId, [FromBody] ProficiencyRequestDto proficiency)
    {
        await EnsureFeatureBelongsToBackground(backgroundId, featureId);
        var updatedFeature = await service.AddProficiency(proficiency, featureId);
        return Ok(mapper.Map<BackgroundFeatureResponseDto>(updatedFeature));
    }

    [HttpDelete("{featureId}/proficiencies")]
    public async Task<ActionResult> RemoveProficiency(int featureId, int backgroundId, [FromBody] ProficiencyRequestDto proficiency)
    {
        await EnsureFeatureBelongsToBackground(backgroundId, featureId);
        await service.RemoveProficiency(proficiency, featureId);
        return Ok();
    }

    // Ability increase management endpoints
    [HttpPost("{featureId}/ability-increases")]
    public async Task<ActionResult<BackgroundFeatureResponseDto>> AddAbilityIncrease(int featureId, int backgroundId, [FromBody] AbilityValueDto increase)
    {
        await EnsureFeatureBelongsToBackground(backgroundId, featureId);
        var updatedFeature = await service.AddAbilityIncrease(increase.AbilityId, increase.Value, featureId);
        return Ok(mapper.Map<BackgroundFeatureResponseDto>(updatedFeature));
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