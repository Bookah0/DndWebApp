using Api.Models.Characters;
namespace Api.Repositories.Interfaces;

public interface IAbilityValueRepository : IRepository<AbilityValue>
{
    Task<AbilityValue> GetWithAbilityAsync(int id);
}