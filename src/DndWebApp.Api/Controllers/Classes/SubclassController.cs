using AutoMapper;
using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Models.DTOs.Character;
using DndWebApp.Api.Models.DTOs.ResponseDtos;
using DndWebApp.Api.Models.DTOs.Spells;
using DndWebApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DndWebApp.Api.Controllers;

[ApiController]
[Route("api/classes/{classId}/subclasses")]
public class SubclassController(ISubclassService service, IClassService classService, IClassLevelService levelService, IMapper mapper) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<ICollection<SubclassResponseDto>>> GetSubclasses(int classId)
    {
        var clss = await classService.GetWithSubclassesAsync(classId);
        return Ok(mapper.Map<ICollection<SubclassResponseDto>>(clss.Subclasses));
    }

    [HttpGet("{subclassId}")]
    public async Task<ActionResult<SubclassResponseDto>> GetSubclass(int subclassId, int classId)
    {
        await EnsureSubclassBelongsToParentClass(classId, subclassId);
        var subclass = await service.GetByIdAsync(subclassId);
        return Ok(mapper.Map<SubclassResponseDto>(subclass));
    }

    [HttpPost]
    public async Task<ActionResult<SubclassResponseDto>> CreateSubclass(int classId, [FromBody] ClassDto dto)
    {
        var subclass = await service.CreateAsync(dto, classId);
        return Ok(mapper.Map<SubclassResponseDto>(subclass));
    }

    [HttpPatch("{subclassId}")]
    public async Task<ActionResult> UpdateSubclass(int subclassId, int classId, [FromBody] ClassDto dto)
    {
        await EnsureSubclassBelongsToParentClass(classId, subclassId);
        await service.UpdateAsync(subclassId, dto);
        return Ok();
    }

    [HttpPatch("{subclassId}/{newParentClassId}")]
    public async Task<ActionResult> UpdateSubclass(int subclassId, int newParentClassId, int classId, [FromBody] ClassDto dto)
    {
        await EnsureSubclassBelongsToParentClass(classId, subclassId);
        await service.UpdateAsync(subclassId, dto, newParentClassId);
        await EnsureSubclassBelongsToParentClass(newParentClassId, subclassId);
        return Ok();
    }

    [HttpDelete("{subclassId}")]
    public async Task<ActionResult> DeleteSubclass(int subclassId, int classId)
    {
        await EnsureSubclassBelongsToParentClass(classId, subclassId);
        await service.DeleteAsync(subclassId);
        return Ok();
    }

    // CRUD Class levels
    [HttpGet("{subclassId}/levels")]
    public async Task<ActionResult<ICollection<ClassLevelResponseDto>>> GetClasses(int subclassId, int classId)
    {
        await EnsureSubclassBelongsToParentClass(classId, subclassId);
        var subclass = await service.GetByIdAsync(subclassId);
        return Ok(mapper.Map<ICollection<ClassLevelResponseDto>>(subclass.ClassLevels));
    }

    [HttpGet("{subclassId}/levels/{levelId}")]
    public async Task<ActionResult<ClassLevelResponseDto>> GetClassLevel(int subclassId, int levelId, int classId)
    {
        await EnsureSubclassBelongsToParentClass(classId, subclassId);
        var level = await levelService.GetByIdAsync(levelId);

        if (level.ClassId != subclassId)
            throw new ValidationException($"Class level with id {level.Id} does not belong to subclass with id {subclassId}");

        return Ok(mapper.Map<ClassLevelResponseDto>(level));
    }

    [HttpPost("{subclassId}/levels")]
    public async Task<ActionResult<ClassLevelResponseDto>> CreateClassLevel(int subclassId, int classId, [FromBody] ClassLevelDto dto)
    {
        await EnsureSubclassBelongsToParentClass(classId, subclassId);
        var classLevel = await levelService.CreateAsync(dto);
        return Ok(mapper.Map<ClassLevelResponseDto>(classLevel));
    }

    [HttpPatch("{subclassId}/levels/{levelId}")]
    public async Task<ActionResult> UpdateClassLevel(int subclassId, int levelId, int classId, [FromBody] ClassLevelDto dto)
    {
        await EnsureSubclassBelongsToParentClass(classId, subclassId);
        await EnsureLevelBelongsToSubclass(subclassId, levelId);
        await levelService.UpdateAsync(levelId, dto);
        return Ok();
    }

    [HttpDelete("{subclassId}/levels/{levelId}")]
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