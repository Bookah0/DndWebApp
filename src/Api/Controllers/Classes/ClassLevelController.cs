using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Api.Models.DTOs.RequestDtos.Character;
using Api.Models.DTOs.ResponseDtos;
using Api.Models.DTOs.Spells;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Classes;

[ApiController]
[Route("api/[controller]/{classId}/levels")]
public class ClassLevelController(IBaseClassService service, IClassLevelService levelService, IMapper mapper) : ControllerBase
    {
    [HttpGet]
    public async Task<ActionResult<ICollection<ClassLevelResponseDto>>> GetClasses(int classId)
    {
        var clss = await service.GetByIdAsync(classId);
        return Ok(mapper.Map<ICollection<ClassLevelResponseDto>>(clss.ClassLevels));
    }

    [HttpGet("{levelId}")]
    public async Task<ActionResult<ClassLevelResponseDto>> GetClassLevel(int classId, int levelId)
    {
        await EnsureLevelBelongsToClass(classId, levelId);
        var clss = await levelService.GetByIdAsync(levelId);
        return Ok(mapper.Map<ClassLevelResponseDto>(clss));
    }

    [HttpPost]
    public async Task<ActionResult<ClassLevelResponseDto>> CreateClassLevel(int classId, [FromBody] CreateClassLevelRequestDto dto)
    {
        if(dto.ClassId != classId)
            throw new ValidationException($"Class id in dto {dto.ClassId} does not match class id in route {classId}");

        var classLevel = await levelService.CreateAsync(dto);
        return Ok(mapper.Map<ClassLevelResponseDto>(classLevel));
    }

    [HttpPatch("{levelId}")]
    public async Task<ActionResult<ClassLevelResponseDto>> UpdateClassLevel(int classId, int levelId, [FromBody] UpdateClassLevelRequestDto dto)
    {
        await EnsureLevelBelongsToClass(classId, levelId);
        var updatedLevel = await levelService.UpdateAsync(levelId, dto);
        return Ok(mapper.Map<ClassLevelResponseDto>(updatedLevel));
    }

    [HttpDelete("{levelId}")]
    public async Task<ActionResult> DeleteClassLevel(int classId, int levelId)
    {
        await EnsureLevelBelongsToClass(classId, levelId);
        await levelService.DeleteAsync(levelId);
        return Ok();
    }

    [HttpPost("{levelId}/features/{featureId}")]
    public async Task<ActionResult<ClassLevelResponseDto>> AddFeatureToClassLevel(int classId, int featureId, int levelId)
    {
        await EnsureLevelBelongsToClass(classId, levelId);
        var updatedLevel = await levelService.AddFeatureAsync(levelId, featureId);
        return Ok(mapper.Map<ClassLevelResponseDto>(updatedLevel));
    }

    [HttpDelete("{levelId}/features/{featureId}")]
    public async Task<ActionResult<ClassLevelResponseDto>> RemoveFeatureFromClassLevel(int classId, int featureId, int levelId)
    {
        await EnsureLevelBelongsToClass(classId, levelId);
        var updatedLevel = await levelService.RemoveFeatureAsync(levelId, featureId);
        return Ok(mapper.Map<ClassLevelResponseDto>(updatedLevel));
    }
    
    [HttpPost("{levelId}/class-slot")]
    public async Task<ActionResult<ClassLevelResponseDto>> AddClassSlotToLevel(int classId, int levelId, [FromBody] ClassSlotRequestDto slot)
    {
        await EnsureLevelBelongsToClass(classId, levelId);
        var updatedLevel = await levelService.AddClassSlotAsync(levelId, slot);
        return Ok(mapper.Map<ClassLevelResponseDto>(updatedLevel));
    }

    [HttpDelete("{levelId}/class-slots/{slotName}")]
    public async Task<ActionResult<ClassLevelResponseDto>> RemoveClassSlotFromLevel(int classId, string slotName, int levelId)
    {
        await EnsureLevelBelongsToClass(classId, levelId);
        var updatedLevel = await levelService.RemoveClassSlotByNameAsync(levelId, slotName);
        return Ok(mapper.Map<ClassLevelResponseDto>(updatedLevel));
    }

    private async Task EnsureLevelBelongsToClass(int classId, int levelId)
    {
        var level = await levelService.GetByIdAsync(levelId);

        if (level.ClassId != classId)
            throw new ValidationException($"Class level with id {level.Id} does not belong to class with id {classId}");
    }
}