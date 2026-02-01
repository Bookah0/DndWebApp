
using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.RequestDtos.Character;
using DndWebApp.Api.Models.World;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Constants;
using DndWebApp.Api.Services.Interfaces;
using DndWebApp.Api.Services.Util;
using static DndWebApp.Api.Services.Util.SortUtil;
using static DndWebApp.Api.Services.Util.ConstantsUtil;

namespace DndWebApp.Api.Services.Implemented;

public class LanguageService(IRepository<Language> repo, ILogger<LanguageService> logger) : ILanguageService
{
    public async Task<Language> CreateAsync(LanguageDto dto)
    {
        var language = await repo.CreateAsync(new()
        {
            Name = dto.Name,
            Script = dto.Script,
            Family = dto.Family,
            IsHomebrew = dto.IsHomebrew,
        });

        return language;
    }

    public async Task DeleteAsync(int id)
    {
        var language = await repo.GetByIdAsync(id) ?? throw new NotFoundException("Language could not be found");
        await repo.DeleteAsync(language);
    }

    public async Task<ICollection<Language>> GetAllAsync()
    {
        var languages = await repo.GetAllAsync();
        return languages;
    }

    public async Task<Language> GetByIdAsync(int id)
    {
        var language = await repo.GetByIdAsync(id) ?? throw new NotFoundException("Language could not be found"); 
        return language;
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

    public ICollection<Language> SortBy(ICollection<Language> languages, string sortFilter, bool descending = false)
    {
        if(!TryResolveOption(sortFilter, SortLanguageOption.AllowedValues, out string? resolved))
            return languages;
    
        return resolved switch
        {
            SortLanguageOption.Name => OrderByMany(languages, [(l => l.Name)], descending),
            SortLanguageOption.Family => OrderByMany(languages, [(l => l.Family), (l => l.Name)], descending),
            SortLanguageOption.Script => OrderByMany(languages, [(l => l.Script), (l => l.Name)], descending),
            _ => throw new ArgumentOutOfRangeException(nameof(sortFilter), "Invalid sort option provided.")
        };
    }
}