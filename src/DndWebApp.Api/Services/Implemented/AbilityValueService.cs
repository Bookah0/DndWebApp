using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.RequestDtos.Character;
using DndWebApp.Api.Services.Interfaces;

namespace DndWebApp.Api.Services.Implemented;

public class AbilityValueService(IAbilityValueRepository repo, IAbilityRepository abilityRepo, ILogger<AbilityValueService> logger) : IAbilityValueService
{
    public async Task<AbilityValue> CreateAsync(AbilityValueDto dto)
    {
        if(dto.Value == 0)
            throw new ValidationException("Ability value cannot be zero");

        var ability = await abilityRepo.GetByIdAsync(dto.AbilityId) 
            ?? throw new NotFoundException("Ability could not be found");

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
        var abilityValue = await repo.GetByIdAsync(id) 
            ?? throw new NotFoundException("Ability value could not be found");

        logger.LogInformation("Deleting ability value, ID: {AbilityValueId}", id);
        await repo.DeleteAsync(abilityValue);
        logger.LogInformation("Successfully deleted ability value, ID: {AbilityValueId}", id);
    }

    public async Task<ICollection<AbilityValue>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<AbilityValue> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) ?? throw new NotFoundException("Ability value could not be found");
    }

    public async Task<AbilityValue> GetWithAbilityAsync(int id)
    {
        return await repo.GetWithAbilityAsync(id) ?? throw new NotFoundException("Ability value could not be found");
    }

    public async Task<AbilityValue> SetValueAsync(int id, int newValue)
    {
        var abilityValue = await repo.GetByIdAsync(id) 
            ?? throw new NotFoundException("Ability value could not be found");

        logger.LogInformation("Updating ability value, ID: {AbilityValueId}, OldValue: {OldValue}, NewValue: {NewValue}", id, abilityValue.Value, newValue);

        abilityValue.Value = newValue;
        await repo.UpdateAsync(abilityValue);

        logger.LogInformation("Successfully updated ability value, ID: {AbilityValueId}, NewValue: {NewValue}", id, newValue);
        return abilityValue;
    }
}