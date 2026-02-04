
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
        logger.LogInformation("Creating language, Name: {LanguageName}", dto.Name);
        var language = await repo.CreateAsync(new()
        {
            Name = dto.Name,
            Script = dto.Script,
            Family = dto.Family,
            IsHomebrew = dto.IsHomebrew,
        });

        logger.LogInformation("Successfully created language, Name: {LanguageName}, ID: {LanguageId}", language.Name, language.Id);
        return language;
    }

    public async Task DeleteAsync(int id)
    {
        var language = await repo.GetByIdAsync(id);
        
        if(!language.IsHomebrew)
            throw new ValidationException("Cannot delete a base language.");
        
        logger.LogInformation("Deleting language with Name: {LanguageName}, ID: {LanguageId}", language.Name, id);
        await repo.DeleteAsync(language);
        logger.LogInformation("Successfully deleted language, Name: {LanguageName}, ID: {LanguageId}", language.Name, id);
    }

    public async Task<ICollection<Language>> GetAllAsync()
    {
        var languages = await repo.GetAllAsync();
        return languages;
    }

    public async Task<Language> GetByIdAsync(int id)
    {
        var language = await repo.GetByIdAsync(id); 
        return language;
    }

    public async Task<Language> UpdateAsync(int id, LanguageDto dto)
    {
        var language = await repo.GetByIdAsync(id);
        logger.LogInformation("Updating language, Name: {LanguageName}, ID: {LanguageId}", language.Name, id);

        language.Name = dto.Name;
        language.Script = dto.Script;
        language.Family = dto.Family;
        language.IsHomebrew = dto.IsHomebrew;

        await repo.UpdateAsync(language);
        logger.LogInformation("Successfully updated language, Name: {LanguageName}, ID: {LanguageId}", language.Name, language.Id);
        return language;
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
            _ => throw new ValidationException($"Invalid sort option: {sortFilter}")
        };
    }
}