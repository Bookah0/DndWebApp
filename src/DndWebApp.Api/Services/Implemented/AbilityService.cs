using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.RequestDtos.Character;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Interfaces;
using static DndWebApp.Api.Services.Util.SortUtil;

namespace DndWebApp.Api.Services.Implemented;

public class AbilityService(IAbilityRepository repo, ILogger<AbilityService> logger) : IAbilityService
{
    public async Task<Ability> CreateAsync(AbilityDto dto)
    {
        Ability ability = new()
        {
            FullName = dto.FullName,
            ShortName = dto.ShortName,
            Description = dto.Description,
            Skills = []
        };

        await repo.CreateAsync(ability);
        return ability;
    }

    public async Task DeleteAsync(int id)
    {
        var ability = await repo.GetByIdAsync(id) ?? throw new NotFoundException("Ability could not be found");
        await repo.DeleteAsync(ability);
    }

    public async Task<ICollection<Ability>> GetAllAsync()
    {
        var abilities = await repo.GetAllAsync();
        return abilities;
    }

    public async Task<Ability> GetByIdAsync(int id)
    {
        var ability = await repo.GetByIdAsync(id) ?? throw new NotFoundException("Ability could not be found");
        return ability;
    }

    public async Task UpdateAsync(int id, AbilityDto dto)
    {
        var ability = await repo.GetByIdAsync(id) ?? throw new NotFoundException("Ability could not be found");

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
        var abilityOrder = CreateOrderLookup(["Strength", "Dexterity", "Constitution", "Intelligence", "Wisdom", "Charisma"]);

        return [.. abilities.OrderBy(a => abilityOrder[a.FullName])];
    }
}