using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Services.Enums;

namespace DndWebApp.Api.Services.Interfaces;

public interface ICharacterService
{
    Task<Character> CreateAsync(CharacterDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Character>> GetAllAsync();
    Task<Character> GetByIdAsync(int id);
    Task LevelUpAsync(ICollection<Spell> chosenSpells, int characterId);
    Task AddSubclassAsync(int subclassId, int characterId);
    Task EditCharacterDescriptionAsync(CharacterDescription edited, int characterId);
    Task SpendHitDice(int nDice, int characterId);
    Task LongRest(int characterId);
    Task TakeDamage(int characterId, int change);
    Task HealDamage(int characterId, int change);
    Task EditCurrentClassSlotAsync(string slotName, int change, int characterId);
    Task EditCurrentSpellSlotAsync(int slotLevel, int change, int characterId);
    ICollection<Character> SortBy(ICollection<Character> characters, CharacterSortFilter sortFilter, bool descending = false);
}