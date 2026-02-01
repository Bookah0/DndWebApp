using System.ComponentModel.DataAnnotations;
using AutoMapper;
using DndWebApp.Api.Models.DTOs.Character;
using DndWebApp.Api.Models.DTOs.ResponseDtos;
using DndWebApp.Api.Models.DTOs.Spells;
using DndWebApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DndWebApp.Api.Controllers;

[ApiController]
[Route("api/classes")]
public class ClassController(IClassService service, IClassLevelService levelService, IMapper mapper) : ControllerBase
    {

    // CRUD Class
    [HttpGet]
    public async Task<ActionResult<ICollection<ClassResponseDto>>> GetClasses()
    {
        var classes = await service.GetAllAsync();
        return Ok(mapper.Map<ICollection<ClassResponseDto>>(classes));
    }

    [HttpGet("{classId}")]
    public async Task<ActionResult<ClassResponseDto>> GetClass(int classId)
    {
        var clss = await service.GetByIdAsync(classId);
        return Ok(mapper.Map<ClassResponseDto>(clss));
    }

    [HttpPost]
    public async Task<ActionResult<ClassResponseDto>> CreateClass([FromBody] ClassDto dto)
    {
        var clss = await service.CreateAsync(dto);
        return Ok(mapper.Map<ClassResponseDto>(clss));
    }

    [HttpPatch("{classId}")]
    public async Task<ActionResult> UpdateClass(int classId, [FromBody] ClassDto dto)
    {
        await service.UpdateAsync(classId, dto);
        return Ok();
    }

    [HttpDelete("{classId}")]
    public async Task<ActionResult> DeleteClass(int classId)
    {
        await service.DeleteAsync(classId);
        return Ok();
    }

    // CRUD Class levels
    [HttpGet("{classId}/levels")]
    public async Task<ActionResult<ICollection<ClassLevelResponseDto>>> GetClasses(int classId)
    {
        var clss = await service.GetByIdAsync(classId);
        return Ok(mapper.Map<ICollection<ClassLevelResponseDto>>(clss.ClassLevels));
    }

    [HttpGet("{classId}/levels/{levelId}")]
    public async Task<ActionResult<ClassLevelResponseDto>> GetClassLevel(int classId, int levelId)
    {
        await EnsureLevelBelongsToClass(classId, levelId);
        var clss = await levelService.GetByIdAsync(levelId);
        return Ok(mapper.Map<ClassLevelResponseDto>(clss));
    }

    [HttpPost("{classId}/levels")]
    public async Task<ActionResult<ClassLevelResponseDto>> CreateClassLevel(int classId, [FromBody] ClassLevelDto dto)
    {
        if(dto.ClassId != classId)
            throw new ValidationException($"Class id in dto {dto.ClassId} does not match class id in route {classId}");

        var classLevel = await levelService.CreateAsync(dto);
        return Ok(mapper.Map<ClassLevelResponseDto>(classLevel));
    }

    [HttpPatch("{classId}/levels/{levelId}")]
    public async Task<ActionResult> UpdateClassLevel(int classId, int levelId, [FromBody] ClassLevelDto dto)
    {
        await EnsureLevelBelongsToClass(classId, levelId);
        await levelService.UpdateAsync(levelId, dto);
        return Ok();
    }

    [HttpDelete("{classId}/levels/{levelId}")]
    public async Task<ActionResult> DeleteClassLevel(int classId, int levelId)
    {
        await EnsureLevelBelongsToClass(classId, levelId);
        await levelService.DeleteAsync(levelId);
        return Ok();
    }

    private async Task EnsureLevelBelongsToClass(int classId, int levelId)
    {
        var level = await levelService.GetByIdAsync(levelId);

        if (level.ClassId != classId)
            throw new ValidationException($"Class level with id {level.Id} does not belong to class with id {classId}");
    }
}