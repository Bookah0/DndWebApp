using System.ComponentModel.DataAnnotations;
using AutoMapper;
using DndWebApp.Api.Models.DTOs.RequestDtos.Character;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.DTOs.ResponseDtos;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Services.Implemented.Classes;
using DndWebApp.Api.Services.Interfaces;
using DndWebApp.Api.Services.Interfaces.Features;
using Microsoft.AspNetCore.Mvc;

namespace DndWebApp.Api.Controllers.Classes;

[ApiController]
[Route("api/classes/{classId}/features")]
public class ClassFeatureController(IFeatureService<ClassFeature, ClassFeatureDto> service, IClassService classService, IMapper mapper) : ControllerBase
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
    public async Task<ActionResult<ClassFeatureResponseDto>> CreateClassFeature(int classId, [FromBody] ClassFeatureDto dto)
    {
        if(dto.ClassId != classId)
            throw new ValidationException($"Class id in dto {dto.ClassId} does not match class id in route {classId}");

        var feature = await service.CreateAsync(dto);
        return Ok(mapper.Map<ClassFeatureResponseDto>(feature));
    }

    [HttpPatch("{featureId}")]
    public async Task<ActionResult> UpdateClassFeature(int classId, int featureId, [FromBody] ClassFeatureDto dto)
    {
        await EnsureFeatureBelongsToClass(classId, featureId);
        await service.UpdateAsync(dto, featureId);
        return Ok();
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
    public async Task<ActionResult> AddSpell(int classId, int featureId, int spellId)
    {
        await EnsureFeatureBelongsToClass(classId, featureId);
        await service.AddSpell(spellId, featureId);
        return Ok();
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
    public async Task<ActionResult> AddProficiency(int classId, int featureId, [FromBody] ProficiencyDto proficiency)
    {
        await EnsureFeatureBelongsToClass(classId, featureId);
        await service.AddProficiency(proficiency, featureId);
        return Ok();
    }

    [HttpDelete("{featureId}/proficiencies")]
    public async Task<ActionResult> RemoveProficiency(int classId, int featureId, [FromBody] ProficiencyDto proficiency)
    {
        await EnsureFeatureBelongsToClass(classId, featureId);
        await service.RemoveProficiency(proficiency, featureId);
        return Ok();
    }

    // Ability increase management endpoints
    [HttpPost("{featureId}/ability-increases")]
    public async Task<ActionResult> AddAbilityIncrease(int classId, int featureId, [FromBody] AbilityValueDto increase)
    {
        await EnsureFeatureBelongsToClass(classId, featureId);
        await service.AddAbilityIncrease(increase.AbilityId, increase.Value, featureId);
        return Ok();
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