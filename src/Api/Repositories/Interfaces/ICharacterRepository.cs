using Api.Models.Characters;
using Api.Models.DTOs.RequestDtos.Character;

namespace Api.Repositories.Interfaces;

public interface ICharacterRepository : IRepository<Character>
{
    Task<CharacterDescription> GetCharacterDescriptionAsync(int id);
    Task<Character> GetWithAllDataAsync(int id);
    Task<Character> GetWithCombatStatsAsync(int characterId);
    Task<Character> GetWithCharacterDescriptionAsync(int characterId);
}