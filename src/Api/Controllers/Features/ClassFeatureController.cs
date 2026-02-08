using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Api.Models.DTOs.RequestDtos.Character;
using Api.Models.DTOs.Features;
using Api.Models.DTOs.ResponseDtos;
using Api.Models.Features;
using Api.Services.Implemented.Classes;
using Api.Services.Interfaces;
using Api.Services.Interfaces.Features;
using Microsoft.AspNetCore.Mvc;
using Dndtoolkit.Api.Models.DTOs.RequestDtos.Features;

namespace Api.Controllers.Classes;

[ApiController]
[Route("api/classes/{classId}/features")]
public class ClassFeatureController(IFeatureService<ClassFeature, CreateClassFeatureRequestDto, UpdateClassFeatureRequestDto> service, IBaseClassService classService, IMapper mapper) : ControllerBase
{
    
    [HttpGet]
    public async Task<ActionResult<ICollection<ClassFeatureResponseDto>>> GetClassFeatures(int classId)
    {
        var clss = await classService.GetWithFeaturesAsync(classId);
        var allFeatures = clss.ClassLevels.SelectMany(cl => cl.NewFeatures).ToList();
        return Ok(mapper.Map<ICollection<ClassFeatureResponseDto>>(allFeatures));
    }

    [HttpGet("{featureId}")]
    public async Task<ActionResult<ClassFeatureResponseDto>> GetClassFeature(int classId, int featureId)
    {
        var feature = await service.GetByIdAsync(featureId);
        
        if (feature.ClassId != classId)
            throw new ValidationException($"Class feature with id {feature.Id} does not belong to class with id {classId}");

        return Ok(mapper.Map<ClassFeatureResponseDto>(feature));
    }

    [HttpPost]
    public async Task<ActionResult<ClassFeatureResponseDto>> CreateClassFeature(int classId, [FromBody] CreateClassFeatureRequestDto dto)
    {
        if(dto.ClassId != classId)
            throw new ValidationException($"Class id in dto {dto.ClassId} does not match class id in route {classId}");

        var feature = await service.CreateAsync(dto);
        return Ok(mapper.Map<ClassFeatureResponseDto>(feature));
    }

    [HttpPatch("{featureId}")]
    public async Task<ActionResult<ClassFeatureResponseDto>> UpdateClassFeature(int classId, int featureId, [FromBody] UpdateClassFeatureRequestDto dto)
    {
        await EnsureFeatureBelongsToClass(classId, featureId);
        var updatedFeature = await service.UpdateAsync(dto, featureId);
        return Ok(mapper.Map<ClassFeatureResponseDto>(updatedFeature));
    }

    [HttpDelete("{featureId}")]
    public async Task<ActionResult> DeleteClassFeature(int classId, int featureId)
    {
        await EnsureFeatureBelongsToClass(classId, featureId);
        await service.DeleteAsync(featureId);
        return Ok();
    }

    // Spell management endpoints
    [HttpPost("{featureId}/spells/{spellId}")]
    public async Task<ActionResult<ClassFeatureResponseDto>> AddSpell(int classId, int featureId, int spellId)
    {
        await EnsureFeatureBelongsToClass(classId, featureId);
        var updatedFeature = await service.AddSpell(spellId, featureId);
        return Ok(mapper.Map<ClassFeatureResponseDto>(updatedFeature));
    }

    [HttpDelete("{featureId}/spells/{spellId}")]
    public async Task<ActionResult> RemoveSpell(int classId, int featureId, int spellId)
    {
        await EnsureFeatureBelongsToClass(classId, featureId);
        await service.RemoveSpell(spellId, featureId);
        return Ok();
    }

    // Proficiency management endpoints
    [HttpPost("{featureId}/proficiencies")]
    public async Task<ActionResult<ClassFeatureResponseDto>> AddProficiency(int classId, int featureId, [FromBody] ProficiencyRequestDto proficiency)
    {
        await EnsureFeatureBelongsToClass(classId, featureId);
        var updatedFeature = await service.AddProficiency(proficiency, featureId);
        return Ok(mapper.Map<ClassFeatureResponseDto>(updatedFeature));
    }

    [HttpDelete("{featureId}/proficiencies")]
    public async Task<ActionResult> RemoveProficiency(int classId, int featureId, [FromBody] ProficiencyRequestDto proficiency)
    {
        await EnsureFeatureBelongsToClass(classId, featureId);
        await service.RemoveProficiency(proficiency, featureId);
        return Ok();
    }

    // Ability increase management endpoints
    [HttpPost("{featureId}/ability-increases")]
    public async Task<ActionResult<ClassFeatureResponseDto>> AddAbilityIncrease(int classId, int featureId, [FromBody] AbilityValueDto increase)
    {
        await EnsureFeatureBelongsToClass(classId, featureId);
        var updatedFeature = await service.AddAbilityIncrease(increase.AbilityId, increase.Value, featureId);
        return Ok(mapper.Map<ClassFeatureResponseDto>(updatedFeature));
    }

    [HttpDelete("{featureId}/ability-increases")]
    public async Task<ActionResult> RemoveAbilityIncrease(int classId, int featureId, [FromBody] AbilityValueDto increase)
    {
        await EnsureFeatureBelongsToClass(classId, featureId);
        await service.RemoveAbilityIncrease(increase.AbilityId, featureId);
        return Ok();
    }

    // Helpers
    private async Task EnsureFeatureBelongsToClass(int classId, int featureId)
    {
        var feature = await service.GetByIdAsync(featureId);

        if (feature.ClassId != classId)
            throw new ValidationException($"Class feature with id {feature.Id} does not belong to class with id {classId}");
    }
}