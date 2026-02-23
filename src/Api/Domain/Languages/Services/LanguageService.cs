using Api.Domain.Languages.Models;
using Api.Domain.Languages.DTOs;
using Api.Domain.Users.Services;
using Api.Infrastructure.Middleware.ExceptionHandling;
using Api.Domain.Shared.DTOs;
using Api.Domain.Languages.Repositories;

namespace Api.Domain.Languages.Services;

public class LanguageService(ILanguageRepository repo, ICurrentUserService currentUserService, ILogger<LanguageService> logger) : ILanguageService
{
    public async Task<Language> CreateAsync(CreateLanguageRequestDto dto)
    {
        logger.LogInformation("Creating language, Name: {LanguageName}", dto.Name);
        var language = await repo.CreateAsync(new()
        {
            Name = dto.Name,
            Script = dto.Script ?? "",
            Family = dto.Family,
            TypicalSpeakers = dto.TypicalSpeakers ?? "",

            CreatedAt = DateTime.UtcNow,
            CreatedBy = currentUserService.GetCurrentUserId(),
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

    public async Task<ICollection<Language>> GetAllAsync() => await repo.GetAllAsync();
    public async Task<Language> GetByIdAsync(int id) => await repo.GetByIdAsync(id);
    public Task<(int, ICollection<Language>)> GetFilteredAsync(LanguageFilterDto filter, PaginationRequestDto pagination)
        => repo.GetFilteredAsync(filter, pagination);

    public async Task<Language> UpdateAsync(int id, UpdateLanguageRequestDto dto)
    {
        var language = await repo.GetByIdAsync(id);
        logger.LogInformation("Updating language, Name: {LanguageName}, ID: {LanguageId}", language.Name, id);

        language.Name = dto.Name ?? language.Name;
        language.Script = dto.Script ?? language.Script;
        language.Family = dto.Family ?? language.Family;
        language.TypicalSpeakers = dto.TypicalSpeakers ?? language.TypicalSpeakers;
        
        language.IsPublic = dto.IsPublic ?? language.IsPublic;
        language.CloningAllowed = dto.CloningAllowed ?? language.CloningAllowed;
        language.UpdatedAt = DateTime.UtcNow;
        
        await repo.UpdateAsync(language);
        logger.LogInformation("Successfully updated language, Name: {LanguageName}, ID: {LanguageId}", language.Name, language.Id);
        return language;
    }
}