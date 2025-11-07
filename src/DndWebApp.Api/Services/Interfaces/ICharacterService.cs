using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Services.Enums;

namespace DndWebApp.Api.Services.Interfaces;

public interface ICharacterService
{
    Task<Character> CreateAsync(CharacterDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Character>> GetAllAsync();
    Task<Character> GetByIdAsync(int id);
    Task LevelUpAsync(int characterId);
    Task AddSubclassAsync(int subclassId, int characterId);
    Task EditCharacterDescriptionAsync(/*EditCharacterDescriptionDto dto*/);
    Task EditCombatStatsAsync(/*EditCombatStatsDto dto*/);
    Task EditCurrentSpellSlotsAsync(/*EditCurrentSpellSlotsDto dto*/);
    ICollection<Character> SortBy(ICollection<Character> characters, CharacterSortFilter sortFilter, bool descending = false);
}