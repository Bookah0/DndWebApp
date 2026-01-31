
using AutoMapper;
using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Models.DTOs.Character;
using DndWebApp.Api.Models.DTOs.ResponseDtos;
using DndWebApp.Api.Models.World;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Enums;
using DndWebApp.Api.Services.Interfaces;
using static DndWebApp.Api.Services.Util.SortUtil;

namespace DndWebApp.Api.Services.Implemented;

public class LanguageService : ILanguageService
{
    private readonly IRepository<Language> repo;
    private readonly ILogger<LanguageService> logger;
    private readonly IMapper mapper;
    
    public LanguageService(IRepository<Language> repo, ILogger<LanguageService> logger, IMapper mapper)
    {
        this.repo = repo;
        this.logger = logger;
        this.mapper = mapper;
    }

    public async Task<LanguageResponseDto> CreateAsync(LanguageDto dto)
    {
        Language language = new()
        {
            Name = dto.Name,
            Script = dto.Script,
            Family = dto.Family,
            IsHomebrew = dto.IsHomebrew,
        };
        var createdLanguage = await repo.CreateAsync(language);
        return mapper.Map<LanguageResponseDto>(createdLanguage);
    }

    public async Task DeleteAsync(int id)
    {
        var language = await repo.GetByIdAsync(id) ?? throw new NotFoundException("Language could not be found");
        await repo.DeleteAsync(language);
    }

    public async Task<ICollection<LanguageResponseDto>> GetAllAsync()
    {
        var languages = await repo.GetAllAsync();
        return mapper.Map<ICollection<LanguageResponseDto>>(languages);
    }

    public async Task<LanguageResponseDto> GetByIdAsync(int id)
    {
        var language = await repo.GetByIdAsync(id) ?? throw new NotFoundException("Language could not be found"); 
        return mapper.Map<LanguageResponseDto>(language);
    }

    public async Task UpdateAsync(int id, LanguageDto dto)
    {
        var language = await repo.GetByIdAsync(id) ?? throw new NotFoundException("Language could not be found");

        language.Name = dto.Name;
        language.Script = dto.Script;
        language.Family = dto.Family;
        language.IsHomebrew = dto.IsHomebrew;

        await repo.UpdateAsync(language);
    }

    public ICollection<Language> SortBy(ICollection<Language> languages, LanguageSortFilter sortFilter, bool descending = false)
    {
        return sortFilter switch
        {
            LanguageSortFilter.Name => OrderByMany(languages, [(l => l.Name)], descending),
            LanguageSortFilter.Family => OrderByMany(languages, [(l => l.Family), (l => l.Name)], descending),
            LanguageSortFilter.Script => OrderByMany(languages, [(l => l.Script), (l => l.Name)], descending),
            _ => languages,
        };
    }
}