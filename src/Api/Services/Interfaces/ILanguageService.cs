
using Api.Models.Characters;
using Api.Models.DTOs.RequestDtos.Character;
using Api.Models.World;

namespace Api.Services.Interfaces;

public interface ILanguageService
{
    Task<Language> CreateAsync(CreateLanguageRequestDto dto); 
    Task DeleteAsync(int id); 
    Task<ICollection<Language>> GetAllAsync(); 
    Task<Language> GetByIdAsync(int id); 
    Task<Language> UpdateAsync(int id, UpdateLanguageRequestDto dto); 
    ICollection<Language> SortBy(ICollection<Language> languages, string sortFilter, bool descending = false);
}