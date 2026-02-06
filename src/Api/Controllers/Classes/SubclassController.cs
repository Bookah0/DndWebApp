using AutoMapper;
using Api.Middlewares.ExceptionHandling;
using Api.Models.DTOs.RequestDtos.Character;
using Api.Models.DTOs.ResponseDtos;
using Api.Models.DTOs.Spells;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Classes;

[ApiController]
[Route("api/classes/{classId}/subclasses")]
public class SubclassController(ISubclassService service, IClassService classService, IMapper mapper) : ControllerBase
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
    public async Task<ActionResult<SubclassResponseDto>> UpdateSubclass(int subclassId, int classId, [FromBody] ClassDto dto)
    {
        await EnsureSubclassBelongsToParentClass(classId, subclassId);
        var updatedSubclass = await service.UpdateAsync(subclassId, dto);
        return Ok(mapper.Map<SubclassResponseDto>(updatedSubclass));
    }

    [HttpPatch("{subclassId}/{newParentClassId}")]
    public async Task<ActionResult<SubclassResponseDto>> UpdateSubclass(int subclassId, int newParentClassId, int classId, [FromBody] ClassDto dto)
    {
        await EnsureSubclassBelongsToParentClass(classId, subclassId);
        var updatedSubclass = await service.UpdateAsync(subclassId, dto);
        await EnsureSubclassBelongsToParentClass(newParentClassId, subclassId);
        return Ok(mapper.Map<SubclassResponseDto>(updatedSubclass));
    }

    [HttpDelete("{subclassId}")]
    public async Task<ActionResult> DeleteSubclass(int subclassId, int classId)
    {
        await EnsureSubclassBelongsToParentClass(classId, subclassId);
        await service.DeleteAsync(subclassId);
        return Ok();
    }

    private async Task EnsureSubclassBelongsToParentClass(int classId, int subclassId)
    {
        var parentClass = await classService.GetWithSubclassesAsync(classId);

        if(!parentClass.Subclasses.Any(sc => sc.Id == subclassId))
            throw new NotFoundException($"Subclass with ID {subclassId} does not belong to Class with ID {classId}.");
    }
}