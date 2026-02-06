using Api.Controllers.Characters;
using Api.Models.Characters;
using Api.Models.DTOs.RequestDtos.Character;

namespace Api.Services.Interfaces;

public interface ICharacterService
{
    Task<Character> CreateAsync(CharacterDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Character>> GetAllAsync();
    Task<Character> GetByIdAsync(int id);
    Task<Character> LevelUpAsync(LevelUpDto dto, int characterId);
    Task<Character> AddSubclassAsync(int subclassId, int characterId);
    Task<Character> EditCharacterDescriptionAsync(CharacterDescription edited, int characterId);
    Task<Character> SpendHitDice(int nDice, int characterId);
    Task<Character> LongRest(int characterId);
    Task<Character> TakeDamage(int characterId, int change);
    Task<Character> HealDamage(int characterId, int change);
    Task<Character> EditCurrentClassSlotAsync(string slotName, int change, int characterId);
    Task<Character> EditCurrentSpellSlotAsync(int slotLevel, int change, int characterId);
    ICollection<Character> SortBy(ICollection<Character> characters, string sortFilter, bool descending = false);
    Task<ICollection<Character>> GetAllByUserIdAsync(int userId);
}