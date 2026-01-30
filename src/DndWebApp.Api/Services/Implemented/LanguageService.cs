
using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Models.DTOs.Character;
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
    
    public LanguageService(IRepository<Language> repo, ILogger<LanguageService> logger)
    {
        this.repo = repo;
        this.logger = logger;
    }

    public async Task<Language> CreateAsync(LanguageDto dto)
    {
        Language language = new()
        {
            Name = dto.Name,
            Script = dto.Script,
            Family = dto.Family,
            IsHomebrew = dto.IsHomebrew,
        };

        return await repo.CreateAsync(language);
    }

    public async Task DeleteAsync(int id)
    {
        var language = await repo.GetByIdAsync(id) ?? throw new NotFoundException("Language could not be found");
        await repo.DeleteAsync(language);
    }

    public async Task<ICollection<Language>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<Language> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) ?? throw new NotFoundException("Language could not be found");
    }

    public async Task UpdateAsync(LanguageDto dto)
    {
        var language = await repo.GetByIdAsync(dto.Id) ?? throw new NotFoundException("Language could not be found");

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