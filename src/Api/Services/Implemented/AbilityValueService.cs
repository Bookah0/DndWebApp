using Api.Middlewares.ExceptionHandling;
using Api.Repositories.Interfaces;
using Api.Models.Characters;
using Api.Models.DTOs.RequestDtos.Character;
using Api.Services.Interfaces;

namespace Api.Services.Implemented;

public class AbilityValueService(IAbilityValueRepository repo, IAbilityRepository abilityRepo, ILogger<AbilityValueService> logger) : IAbilityValueService
{
    public async Task<AbilityValue> CreateAsync(AbilityValueDto dto)
    {
        if(dto.Value == 0)
            throw new ValidationException("Ability value cannot be zero");

        var ability = await abilityRepo.GetByIdAsync(dto.AbilityId) ;

        logger.LogInformation("Creating ability value, AbilityName: {AbilityName}, Value: {Value}", ability.FullName, dto.Value);

        var abilityValue = await repo.CreateAsync(new()
        {
            Value = dto.Value,
            AbilityId = dto.AbilityId,
            Ability = ability,
        });
        logger.LogInformation("Successfully created ability value, AbilityName: {AbilityName}, Value: {Value}", ability.FullName, dto.Value);
        return abilityValue;
    }

    public async Task DeleteAsync(int id)
    {
        var abilityValue = await repo.GetByIdAsync(id);
        logger.LogInformation("Deleting ability value, ID: {AbilityValueId}", id);
        await repo.DeleteAsync(abilityValue);
        logger.LogInformation("Successfully deleted ability value, ID: {AbilityValueId}", id);
    }

    public async Task<ICollection<AbilityValue>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<AbilityValue> GetByIdAsync(int id) => await repo.GetByIdAsync(id);

    public async Task<AbilityValue> GetWithAbilityAsync(int id) => await repo.GetWithAbilityAsync(id);

    public async Task<AbilityValue> SetValueAsync(int id, int newValue)
    {
        var abilityValue = await repo.GetByIdAsync(id);

        logger.LogInformation("Updating ability value, ID: {AbilityValueId}, OldValue: {OldValue}, NewValue: {NewValue}", id, abilityValue.Value, newValue);
        abilityValue.Value = newValue;
        await repo.UpdateAsync(abilityValue);

        logger.LogInformation("Successfully updated ability value, ID: {AbilityValueId}, NewValue: {NewValue}", id, newValue);
        return abilityValue;
    }
}