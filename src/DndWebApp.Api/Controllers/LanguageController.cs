using AutoMapper;
using DndWebApp.Api.Models.DTOs.Character;
using DndWebApp.Api.Models.DTOs.ResponseDtos;
using DndWebApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DndWebApp.Api.Controllers;

[ApiController]
[Route("api/[controller]s")]
public class LanguageController(ILanguageService service, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ICollection<LanguageResponseDto>>> GetLanguages()
    {
        var languages = await service.GetAllAsync();
        return Ok(mapper.Map<ICollection<LanguageResponseDto>>(languages));
    }

    [HttpGet("{languageId}")]
    public async Task<ActionResult<LanguageResponseDto>> GetLanguage(int languageId )
    {
        var language = await service.GetByIdAsync(languageId);
        return Ok(mapper.Map<LanguageResponseDto>(language));
    }

    [HttpPost]
    public async Task<ActionResult<LanguageResponseDto>> CreateLanguage([FromBody] LanguageDto dto)
    {
        var language = await service.CreateAsync(dto);
        return Ok(mapper.Map<LanguageResponseDto>(language));
    }

    [HttpPatch("{languageId}")]
    public async Task<ActionResult> UpdateLanguage(int languageId, [FromBody] LanguageDto dto)
    {
        await service.UpdateAsync(languageId, dto);
        return Ok();
    }

    [HttpDelete("{languageId}")]
    public async Task<ActionResult> DeleteLanguage(int languageId)
    {
        await service.DeleteAsync(languageId);
        return Ok();
    }
}