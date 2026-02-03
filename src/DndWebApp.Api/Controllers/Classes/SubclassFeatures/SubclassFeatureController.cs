using AutoMapper;
using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Models.DTOs.RequestDtos.Character;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.DTOs.ResponseDtos;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Services.Interfaces;
using DndWebApp.Api.Services.Interfaces.Features;
using Microsoft.AspNetCore.Mvc;

namespace DndWebApp.Api.Controllers.Classes;

[ApiController]
[Route("api/classes/{classId}/subclasses/{subclassId}/features")]
public class SubclassFeatureController(
    IFeatureService<ClassFeature, ClassFeatureDto> service, 
    ISubclassService subclassService, 
    IClassService classService, 
    IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ICollection<ClassFeatureResponseDto>>> GetClassFeatures(int subclassId, int classId)
    {
        await EnsureSubclassBelongsToParentClass(classId, subclassId);
        var subclass =  await subclassService.GetWithFeaturesAsync(subclassId);
        var allFeatures = subclass.ClassLevels.SelectMany(cl => cl.NewFeatures).ToList();
        return Ok(mapper.Map<ICollection<ClassFeatureResponseDto>>(allFeatures));
    }

    [HttpGet("{featureId}")]
    public async Task<ActionResult<ClassFeatureResponseDto>> GetClassFeature(int subclassId, int featureId, int classId)
    {
        await EnsureSubclassBelongsToParentClass(classId, subclassId);
        var feature = await service.GetByIdAsync(featureId);

        if (feature.ClassId != subclassId)
            throw new ValidationException($"Subclass feature with id {feature.Id} does not belong to subclass with id {subclassId}");

        return Ok(mapper.Map<ClassFeatureResponseDto>(feature));
    }

    [HttpPost]
    public async Task<ActionResult<ClassFeatureResponseDto>> CreateClassFeature(int subclassId, int classId, [FromBody] ClassFeatureDto dto)
    {
        await EnsureSubclassBelongsToParentClass(classId, subclassId);
        var subclass =  await subclassService.GetWithLevelsAsync(subclassId);

        if(!subclass.ClassLevels.Any(scl => scl.Id == dto.LevelId)) 
            throw new NotFoundException($"Class level with id {dto.LevelId} could not be found in subclass with id {subclassId}");

        if (dto.ClassId != classId)
            throw new ValidationException($"Class level with id {dto.LevelId} does not belong to class with id {classId}");

        var classFeature = await service.CreateAsync(dto);
        return Ok(mapper.Map<ClassFeatureResponseDto>(classFeature));
    }

    [HttpPatch("{featureId}")]
    public async Task<ActionResult> UpdateClassFeature(int subclassId, int featureId, int classId, [FromBody] ClassFeatureDto dto)
    {
        await EnsureFeatureBelongsToSubclass(classId, subclassId, featureId);
        await service.UpdateAsync(dto, featureId);
        return Ok();
    }

    [HttpDelete("{featureId}")]
    public async Task<ActionResult> DeleteClassFeature(int subclassId, int featureId, int classId)
    {
        await EnsureFeatureBelongsToSubclass(classId, subclassId, featureId);
        await service.DeleteAsync(featureId);
        return Ok();
    }

    // Spell management endpoints
    [HttpPost("{featureId}/spells/{spellId}")]
    public async Task<ActionResult> AddSpell(int subclassId, int featureId, int spellId, int classId)
    {
        await EnsureFeatureBelongsToSubclass(classId, subclassId, featureId);
        await service.AddSpell(spellId, featureId);
        return Ok();
    }

    [HttpDelete("{featureId}/spells/{spellId}")]
    public async Task<ActionResult> RemoveSpell(int subclassId, int featureId, int spellId, int classId)
    {
        await EnsureFeatureBelongsToSubclass(classId, subclassId, featureId);
        await service.RemoveSpell(spellId, featureId);
        return Ok();
    }

    // Proficiency management endpoints
    [HttpPost("{featureId}/proficiencies")]
    public async Task<ActionResult> AddProficiency(int subclassId, int featureId, int classId, [FromBody] ProficiencyDto proficiency)
    {
        await EnsureFeatureBelongsToSubclass(classId, subclassId, featureId);
        await service.AddProficiency(proficiency, featureId);
        return Ok();
    }

    [HttpDelete("{featureId}/proficiencies")]
    public async Task<ActionResult> RemoveProficiency(int subclassId, int featureId, int classId, [FromBody] ProficiencyDto proficiency)
    {
        await EnsureFeatureBelongsToSubclass(classId, subclassId, featureId);
        await service.RemoveProficiency(proficiency, featureId);
        return Ok();
    }

    // Ability increase management endpoints
    [HttpPost("{featureId}/ability-increases")]
    public async Task<ActionResult> AddAbilityIncrease(int subclassId, int featureId, int classId, [FromBody] AbilityValueDto increase)
    {
        await EnsureFeatureBelongsToSubclass(classId, subclassId, featureId);
        await service.AddAbilityIncrease(increase.AbilityId, increase.Value, featureId);
        return Ok();
    }

    [HttpDelete("{featureId}/ability-increases")]
    public async Task<ActionResult> RemoveAbilityIncrease(int subclassId, int featureId, int classId, [FromBody] AbilityValueDto increase)
    {
        await EnsureFeatureBelongsToSubclass(classId, subclassId, featureId);
        await service.RemoveAbilityIncrease(increase.AbilityId, featureId);
        return Ok();
    }

    // Helpers
    private async Task EnsureSubclassBelongsToParentClass(int classId, int subclassId)
    {
        var parentClass = await classService.GetWithSubclassesAsync(classId);

        if(!parentClass.Subclasses.Any(sc => sc.Id == subclassId))
            throw new NotFoundException($"Subclass with ID {subclassId} does not belong to Class with ID {classId}.");
    }

    private async Task EnsureFeatureBelongsToSubclass(int classId, int subclassId, int featureId)
    {
        await EnsureSubclassBelongsToParentClass(classId, subclassId);
        var feature = await service.GetByIdAsync(featureId);

        if (feature.ClassId != subclassId)
            throw new ValidationException($"Subclass feature with id {feature.Id} does not belong to subclass with id {subclassId}");
    }
}