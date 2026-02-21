using Api.Domain.Abilities.Models;
using Api.Domain.Shared.Repositories;

namespace Api.Domain.Abilities.Repositories;

public interface IAbilityValueRepository : IRepository<AbilityValue>
{
    Task<AbilityValue> GetWithAbilityAsync(int id);
}