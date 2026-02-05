using AutoMapper;
using Api.Models.DTOs.RequestDtos.Character;
using Api.Models.DTOs.ResponseDtos;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

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
    public async Task<ActionResult<LanguageResponseDto>> UpdateLanguage(int languageId, [FromBody] LanguageDto dto)
    {
        var updatedLanguage = await service.UpdateAsync(languageId, dto);
        return Ok(mapper.Map<LanguageResponseDto>(updatedLanguage));
    }

    [HttpDelete("{languageId}")]
    public async Task<ActionResult> DeleteLanguage(int languageId)
    {
        await service.DeleteAsync(languageId);
        return Ok();
    }
}