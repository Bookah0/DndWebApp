using Api.Domain.Classes.DTOs;
using Api.Domain.Classes.Services;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Utils;
using Api.Infrastructure.Middleware.ExceptionHandling;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Api.Domain.Classes.Controllers;

[ApiController]
[Route("api/classes/{classId}/subclasses")]
public class SubclassController(ISubclassService service, IBaseClassService classService, IMapper mapper) : ControllerBase
{
    [HttpGet("/api/subclasses")]
    public async Task<ActionResult<ICollection<SubclassResponseDto>>> GetAllSubclasses([FromQuery] SubclassFilterDto filter, [FromQuery] PaginationRequestDto pagination)
    {
        var (totalCount, subclasses) = await service.GetFilteredAsync(filter, pagination);
        
        return Ok(new PaginationResponseDto<SubclassResponseDto>
        {
            Items = mapper.Map<ICollection<SubclassResponseDto>>(subclasses),
            ItemCount = totalCount,
            Page = pagination.Page,
            PageSize = pagination.PageSize,
            Next = PaginationUtil.GetNext(pagination, totalCount, "api/subclasses"),
            Prev = PaginationUtil.GetPrev(pagination, "api/subclasses")
        });
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<SubclassResponseDto>>> GetSubclassesByParent(int classId)
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
    public async Task<ActionResult<SubclassResponseDto>> CreateSubclass(int classId, [FromBody] CreateSubclassRequestDto dto)
    {
        var subclass = await service.CreateAsync(dto, classId);
        return Ok(mapper.Map<SubclassResponseDto>(subclass));
    }

    [HttpPatch("{subclassId}")]
    public async Task<ActionResult<SubclassResponseDto>> UpdateSubclass(int subclassId, int classId, [FromBody] UpdateSubclassRequestDto dto)
    {
        await EnsureSubclassBelongsToParentClass(classId, subclassId);
        var updatedSubclass = await service.UpdateAsync(subclassId, dto);

        if(dto.NewParentClassId.HasValue)
            await EnsureSubclassBelongsToParentClass(dto.NewParentClassId.Value, subclassId);

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