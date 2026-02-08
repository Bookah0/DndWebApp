using Api.Controllers.Characters;
using Api.Models.Characters;
using Api.Models.DTOs.RequestDtos.Character;

namespace Api.Services.Interfaces;

public interface ICharacterService
{
    Task<Character> CreateAsync(CreateCharacterRequestDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Character>> GetAllAsync();
    Task<Character> GetByIdAsync(int id);
    Task<Character> LevelUpAsync(LevelUpDto dto, int characterId);
    Task<Character> ChangeClassAsync(int subclassId, int characterId);
    Task<Character> EditCharacterInfoAsync(CharacterInfo edited, int characterId);
    Task<Character> SpendHitDiceAsync(int nDice, int characterId);
    Task<Character> LongRestAsync(int characterId);
    Task<Character> TakeDamageAsync(int characterId, int change);
    Task<Character> HealDamageAsync(int characterId, int change);
    Task<Character> EditCurrentClassSlotAsync(string slotName, int change, int characterId);
    Task<Character> EditCurrentSpellSlotAsync(int slotLevel, int change, int characterId);
    ICollection<Character> SortBy(ICollection<Character> characters, string sortFilter, bool descending = false);
    Task<ICollection<Character>> GetAllByUserIdAsync(Guid userId);
    Task<ICollection<Character>> GetAllByCurrentUserAsync();
}