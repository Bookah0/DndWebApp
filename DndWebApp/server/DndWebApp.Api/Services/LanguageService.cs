using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.World;
using DndWebApp.Api.Repositories;
using DndWebApp.Api.Repositories.Abilities;
using DndWebApp.Api.Services.Util;
namespace DndWebApp.Api.Services;

public class LanguageService : IService<Language, LanguageDto, LanguageDto>
{
    protected IRepository<Language> repo;
    protected AppDbContext context;
    public LanguageService(IRepository<Language> repo, AppDbContext context)
    {
        this.context = context;
        this.repo = repo;
    }

    public async Task<Language> CreateAsync(LanguageDto dto)
    {
        ValidationUtil.ValidateRequiredString(dto.Name);
        ValidationUtil.ValidateRequiredString(dto.Script);
        ValidationUtil.ValidateRequiredString(dto.Family);

        Language language = new()
        {
            Name = dto.Name,
            Script = dto.Script,
            Family = dto.Family,
            IsHomebrew = dto.IsHomebrew,
        };
        
        await repo.CreateAsync(language);
        await context.SaveChangesAsync();
        return language;
    }

    public async Task DeleteAsync(int id)
    {
        var language = await repo.GetByIdAsync(id) ?? throw new NullReferenceException("Language could not be found");
        await repo.DeleteAsync(language);
        await context.SaveChangesAsync();
    }

    public async Task<ICollection<Language>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<Language> GetByIdAsync(int id)
    {
        var language = await repo.GetByIdAsync(id) ?? throw new NullReferenceException("Language could not be found");
        return language;
    }

    public async Task UpdateAsync(LanguageDto dto)
    {
        ValidationUtil.ValidateRequiredString(dto.Name);
        ValidationUtil.ValidateRequiredString(dto.Script);
        ValidationUtil.ValidateRequiredString(dto.Family);

        var language = await repo.GetByIdAsync(dto.Id) ?? throw new NullReferenceException("Language could not be found");

        language.Name = dto.Name;
        language.Script = dto.Script;
        language.Family = dto.Family;
        language.IsHomebrew = dto.IsHomebrew;
        
        await repo.UpdateAsync(language);
        await context.SaveChangesAsync();
    }
}