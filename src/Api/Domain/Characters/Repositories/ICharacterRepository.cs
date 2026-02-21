using Api.Domain.Characters.Models;
using Api.Domain.Shared.Repositories;

namespace Api.Domain.Characters.Repositories;

public interface ICharacterRepository : IRepository<Character>
{
    Task<CharacterInfo> GetCharacterInfoAsync(int id);
    Task<Character> GetWithInventoryAsync(int id);
    Task<Character> GetWithAllDataAsync(int id);
    Task<Character> GetWithCombatStatsAsync(int characterId);
    Task<Character> GetWithCharacterInfoAsync(int characterId);
    Task<Character> GetWithClassesAsync(int id);
    Task<Character> GetWithFeaturesAsync(int id);
}