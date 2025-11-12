using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;

namespace DndWebApp.Api.Repositories.Interfaces;

public interface ICharacterRepository : IRepository<Character>
{
    Task<CharacterDescriptionDto?> GetCharacterDescriptionAsync(int id);
    Task<Character?> GetWithAllDataAsync(int id);
    Task<Character?> GetWithCombatStatsAsync(int characterId);
    Task<Character?> GetWithCharacterDescriptionAsync(int characterId);
}