
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.RequestDtos.Character;
using DndWebApp.Api.Models.World;

namespace DndWebApp.Api.Services.Interfaces;

public interface ILanguageService
{
    Task<Language> CreateAsync(LanguageDto dto); 
    Task DeleteAsync(int id); 
    Task<ICollection<Language>> GetAllAsync(); 
    Task<Language> GetByIdAsync(int id); 
    Task UpdateAsync(int id, LanguageDto dto); 
    ICollection<Language> SortBy(ICollection<Language> languages, string sortFilter, bool descending = false);
}