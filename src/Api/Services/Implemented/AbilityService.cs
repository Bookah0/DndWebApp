using Api.Middlewares.ExceptionHandling;
using Api.Models.Characters;
using Api.Models.DTOs.RequestDtos.Character;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;
using static Api.Services.Util.SortUtil;

namespace Api.Services.Implemented;

public class AbilityService(IAbilityRepository repo, ILogger<AbilityService> logger) : IAbilityService
{
    public async Task<Ability> CreateAsync(AbilityDto dto)
    {
        logger.LogInformation("Creating ability, FullName: {AbilityFullName}", dto.FullName);
        
        Ability ability = await repo.CreateAsync(new()
        {
            FullName = dto.FullName,
            ShortName = dto.ShortName,
            Description = dto.Description,
            Skills = []
        });

        logger.LogInformation("Successfully created ability, FullName: {AbilityFullName}, ID: {AbilityId}", ability.FullName, ability.Id);
        return ability;
    }

    public async Task DeleteAsync(int id)
    {
        var ability = await repo.GetByIdAsync(id);
        logger.LogInformation("Deleting ability with FullName: {AbilityFullName}, ID: {AbilityId}", ability.FullName, id);
        await repo.DeleteAsync(ability);
        logger.LogInformation("Successfully deleted ability, FullName: {AbilityFullName}, ID: {AbilityId}", ability.FullName, id);
    }

    public async Task<ICollection<Ability>> GetAllAsync() => await repo.GetAllAsync();
    public async Task<Ability> GetByIdAsync(int id) => await repo.GetByIdAsync(id);

    public async Task<Ability> UpdateAsync(int id, AbilityDto dto)
    {
        var ability = await repo.GetByIdAsync(id);
        logger.LogInformation("Updating ability, FullName: {AbilityFullName}, ID: {AbilityId}", ability.FullName, id);

        ability.FullName = dto.FullName;
        ability.ShortName = dto.ShortName;
        ability.Description = dto.Description;

        await repo.UpdateAsync(ability);
        logger.LogInformation("Successfully updated ability, FullName: {AbilityFullName}, ID: {AbilityId}", ability.FullName, id);
        return ability;
    }

    public int GetModifier(AbilityValue val)
    {
        return val.Value - 10 / 2;
    }

    // TODO Move to database level sorting
    public ICollection<Ability> SortBy(ICollection<Ability> abilities)
    {
        var abilityOrder = CreateOrderLookup(["Strength", "Dexterity", "Constitution", "Intelligence", "Wisdom", "Charisma"]);

        return [.. abilities.OrderBy(a => abilityOrder[a.FullName])];
    }
}