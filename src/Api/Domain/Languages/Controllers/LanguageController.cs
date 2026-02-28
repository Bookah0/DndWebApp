using Api.Domain.Languages.DTOs;
using Api.Domain.Languages.Services;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Api.Domain.Languages.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LanguagesController(ILanguageService service, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ICollection<LanguageResponseDto>>> GetLanguages([FromQuery] LanguageFilterDto? filter = null, [FromQuery] PaginationRequestDto? pagination = null)
    {
        var languages = await service.GetAllAsync(filter, pagination);
		var mappedLanguages = mapper.Map<ICollection<LanguageResponseDto>>(languages);

		return Ok(PaginationUtil.BuildPaginationResponse(mappedLanguages, pagination, "api/languages"));
    }

    [HttpGet("{languageId}")]
    public async Task<ActionResult<LanguageResponseDto>> GetLanguage(int languageId )
    {
        var language = await service.GetByIdAsync(languageId);
        return Ok(mapper.Map<LanguageResponseDto>(language));
    }

    [HttpPost]
    public async Task<ActionResult<LanguageResponseDto>> CreateLanguage([FromBody] CreateLanguageRequestDto dto)
    {
        var language = await service.CreateAsync(dto);
        return Ok(mapper.Map<LanguageResponseDto>(language));
    }

    [HttpPatch("{languageId}")]
    public async Task<ActionResult<LanguageResponseDto>> UpdateLanguage(int languageId, [FromBody] UpdateLanguageRequestDto dto)
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