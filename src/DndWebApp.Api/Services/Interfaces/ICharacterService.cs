using DndWebApp.Api.Controllers.Characters;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.RequestDtos.Character;

namespace DndWebApp.Api.Services.Interfaces;

public interface ICharacterService
{
    Task<Character> CreateAsync(CharacterDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Character>> GetAllAsync();
    Task<Character> GetByIdAsync(int id);
    Task LevelUpAsync(LevelUpDto dto, int characterId);
    Task AddSubclassAsync(int subclassId, int characterId);
    Task EditCharacterDescriptionAsync(CharacterDescription edited, int characterId);
    Task SpendHitDice(int nDice, int characterId);
    Task LongRest(int characterId);
    Task TakeDamage(int characterId, int change);
    Task HealDamage(int characterId, int change);
    Task EditCurrentClassSlotAsync(string slotName, int change, int characterId);
    Task EditCurrentSpellSlotAsync(int slotLevel, int change, int characterId);
    ICollection<Character> SortBy(ICollection<Character> characters, string sortFilter, bool descending = false);
    Task<ICollection<Character>> GetAllByUserIdAsync(int userId);
}