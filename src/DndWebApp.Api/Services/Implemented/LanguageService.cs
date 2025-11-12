using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.World;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Enums;
using DndWebApp.Api.Services.Interfaces;
using DndWebApp.Api.Services.Util;
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
        ValidationUtil.HasContentOrThrow(dto.Name);
        ValidationUtil.HasContentOrThrow(dto.Script);
        ValidationUtil.HasContentOrThrow(dto.Family);

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
        var language = await repo.GetByIdAsync(id) ?? throw new NullReferenceException("Language could not be found");
        await repo.DeleteAsync(language);
    }

    public async Task<ICollection<Language>> GetAllAsync()
    {
        return await repo.GetMiscellaneousItemsAsync();
    }

    public async Task<Language> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) ?? throw new NullReferenceException("Language could not be found");
    }

    public async Task UpdateAsync(LanguageDto dto)
    {
        ValidationUtil.HasContentOrThrow(dto.Name);
        ValidationUtil.HasContentOrThrow(dto.Script);
        ValidationUtil.HasContentOrThrow(dto.Family);

        var language = await repo.GetByIdAsync(dto.Id) ?? throw new NullReferenceException("Language could not be found");

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
            LanguageSortFilter.Name => SortUtil.OrderByMany(languages, [(l => l.Name)], descending),
            LanguageSortFilter.Family => SortUtil.OrderByMany(languages, [(l => l.Family), (l => l.Name)], descending),
            LanguageSortFilter.Script => SortUtil.OrderByMany(languages, [(l => l.Script), (l => l.Name)], descending),
            _ => languages,
        };
    }
}