using static Api.Domain.Shared.Utils.QueryExtensions;
using Api.Domain.Abilities.DTOs;
using Api.Domain.Abilities.Models;
using Api.Domain.Abilities.Repositories;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Utils;

namespace Api.Domain.Abilities.Services;

public class AbilityService(IAbilityRepository repo, ILogger<AbilityService> logger) : IAbilityService
{
    public async Task<Ability> CreateAsync(AbilityRequestDto dto)
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
    public async Task<ICollection<Ability>> GetAllAsync(AbilityFilterDto? filter = null)
        => await repo.GetAllAsync(filter);
    
    public async Task<ICollection<Ability>> GetAllAsync() => await repo.GetAllAsync();
    public async Task<Ability> GetByIdAsync(int id) => await repo.GetByIdAsync(id);

    public async Task<Ability> UpdateAsync(int id, AbilityRequestDto dto)
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
}