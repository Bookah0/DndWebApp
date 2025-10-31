using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Repositories.Abilities;
using DndWebApp.Api.Repositories.Skills;
using DndWebApp.Api.Services.Generic;
using DndWebApp.Api.Services.Util;
namespace DndWebApp.Api.Services;

public class AbilityService : IService<Ability, AbilityDto, AbilityDto>
{
    protected IAbilityRepository repo;

    public AbilityService(IAbilityRepository repo)
    {
        this.repo = repo;
    }

    public async Task<Ability> CreateAsync(AbilityDto dto)
    {
        ValidationUtil.ValidateRequiredString(dto.FullName);
        ValidationUtil.ValidateRequiredString(dto.ShortName);
        ValidationUtil.ValidateRequiredString(dto.Description);

        Ability ability = new()
        {
            FullName = dto.FullName,
            ShortName = dto.ShortName,
            Description = dto.Description,
            Skills = []
        };

        
        return await repo.CreateAsync(ability);
    }

    public async Task DeleteClassLevelAsync(int id)
    {
        var ability = await repo.GetByIdAsync(id) ?? throw new NullReferenceException("Ability could not be found");
        await repo.DeleteAsync(ability);
    }

    public async Task<ICollection<Ability>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<Ability> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) ?? throw new NullReferenceException("Ability could not be found");
    }

    public async Task UpdateAsync(AbilityDto dto)
    {
        ValidationUtil.ValidateRequiredString(dto.FullName);
        ValidationUtil.ValidateRequiredString(dto.ShortName);
        ValidationUtil.ValidateRequiredString(dto.Description);

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