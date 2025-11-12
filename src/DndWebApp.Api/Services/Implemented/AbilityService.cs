using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Interfaces;
using DndWebApp.Api.Services.Util;

namespace DndWebApp.Api.Services.Implemented;

public class AbilityService : IAbilityService
{
    private readonly IAbilityRepository repo;
    private readonly ILogger<AbilityService> logger;

    public AbilityService(IAbilityRepository repo, ILogger<AbilityService> logger)
    {
        this.repo = repo;
        this.logger = logger;
    }

    public async Task<Ability> CreateAsync(AbilityDto dto)
    {
        ValidationUtil.HasContentOrThrow(dto.FullName);
        ValidationUtil.HasContentOrThrow(dto.ShortName);
        ValidationUtil.HasContentOrThrow(dto.Description);

        Ability ability = new()
        {
            FullName = dto.FullName,
            ShortName = dto.ShortName,
            Description = dto.Description,
            Skills = []
        };


        return await repo.CreateAsync(ability);
    }

    public async Task DeleteAsync(int id)
    {
        var ability = await repo.GetByIdAsync(id) ?? throw new NullReferenceException("Ability could not be found");
        await repo.DeleteAsync(ability);
    }

    public async Task<ICollection<Ability>> GetAllAsync()
    {
        return await repo.GetMiscellaneousItemsAsync();
    }

    public async Task<Ability> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) ?? throw new NullReferenceException("Ability could not be found");
    }

    public async Task UpdateAsync(AbilityDto dto)
    {
        ValidationUtil.HasContentOrThrow(dto.FullName);
        ValidationUtil.HasContentOrThrow(dto.ShortName);
        ValidationUtil.HasContentOrThrow(dto.Description);

        var ability = await repo.GetByIdAsync(dto.Id) ?? throw new NullReferenceException("Ability could not be found");

        ability.FullName = dto.FullName;
        ability.ShortName = dto.ShortName;
        ability.Description = dto.Description;

        await repo.UpdateAsync(ability);
    }

    public int GetModifier(AbilityValue val)
    {
        return val.Value - 10 / 2;
    }

    public ICollection<Ability> SortBy(ICollection<Ability> abilities)
    {
        var abilityOrder = SortUtil.CreateOrderLookup(["Strength", "Dexterity", "Constitution", "Intelligence", "Wisdom", "Charisma"]);

        return [.. abilities.OrderBy(a => abilityOrder[a.FullName])];
    }
}