using AutoMapper;
using Api.Middlewares.ExceptionHandling;
using Api.Models.DTOs.RequestDtos.Character;
using Api.Models.DTOs.ResponseDtos;
using Api.Models.DTOs.Spells;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Classes;

[ApiController]
[Route("api/classes/{classId}/[controller]/{subclassId}/levels")]
public class SubclassLevelController(ISubclassService service, IBaseClassService classService, IClassLevelService levelService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ICollection<ClassLevelResponseDto>>> GetClasses(int subclassId, int classId)
    {
        await EnsureSubclassBelongsToParentClass(classId, subclassId);
        var subclass = await service.GetByIdAsync(subclassId);
        return Ok(mapper.Map<ICollection<ClassLevelResponseDto>>(subclass.ClassLevels));
    }

    [HttpGet("{levelId}")]
    public async Task<ActionResult<ClassLevelResponseDto>> GetClassLevel(int subclassId, int levelId, int classId)
    {
        await EnsureSubclassBelongsToParentClass(classId, subclassId);
        var level = await levelService.GetByIdAsync(levelId);

        if (level.ClassId != subclassId)
            throw new ValidationException($"Class level with id {level.Id} does not belong to subclass with id {subclassId}");

        return Ok(mapper.Map<ClassLevelResponseDto>(level));
    }

    [HttpPost]
    public async Task<ActionResult<ClassLevelResponseDto>> CreateClassLevel(int subclassId, int classId, [FromBody] CreateClassLevelRequestDto dto)
    {
        await EnsureSubclassBelongsToParentClass(classId, subclassId);
        var classLevel = await levelService.CreateAsync(dto);
        return Ok(mapper.Map<ClassLevelResponseDto>(classLevel));
    }

    [HttpPatch("{levelId}")]
    public async Task<ActionResult<ClassLevelResponseDto>> UpdateClassLevel(int subclassId, int levelId, int classId, [FromBody] UpdateClassLevelRequestDto dto)
    {
        await EnsureSubclassBelongsToParentClass(classId, subclassId);
        await EnsureLevelBelongsToSubclass(subclassId, levelId);
        var updatedLevel = await levelService.UpdateAsync(levelId, dto);
        return Ok(mapper.Map<ClassLevelResponseDto>(updatedLevel));
    }

    [HttpDelete("{levelId}")]
    public async Task<ActionResult> DeleteClassLevel(int subclassId, int levelId, int classId)
    {
        await EnsureSubclassBelongsToParentClass(classId, subclassId);
        await EnsureLevelBelongsToSubclass(subclassId, levelId);
        await levelService.DeleteAsync(levelId);
        return Ok();
    }

    private async Task EnsureSubclassBelongsToParentClass(int classId, int subclassId)
    {
        var parentClass = await classService.GetWithSubclassesAsync(classId);

        if(!parentClass.Subclasses.Any(sc => sc.Id == subclassId))
            throw new NotFoundException($"Subclass with ID {subclassId} does not belong to Class with ID {classId}.");
    }

    private async Task EnsureLevelBelongsToSubclass(int subclassId, int levelId)
    {
        var level = await levelService.GetByIdAsync(levelId);

        if (level.ClassId != subclassId)
            throw new ValidationException($"Class level with id {level.Id} does not belong to subclass with id {subclassId}");
    }
}