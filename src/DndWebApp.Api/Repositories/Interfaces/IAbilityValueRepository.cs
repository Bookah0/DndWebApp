using DndWebApp.Api.Models.Characters;
namespace DndWebApp.Api.Repositories.Interfaces;

public interface IAbilityValueRepository : IRepository<AbilityValue>
{
    Task<AbilityValue?> GetWithAbilityAsync(int id);
}