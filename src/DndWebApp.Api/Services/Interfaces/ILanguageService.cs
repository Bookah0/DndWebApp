
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.Character;
using DndWebApp.Api.Services.Constants;

namespace DndWebApp.Api.Services.Interfaces;

public interface ILanguageService
{
    Task<Language> CreateAsync(LanguageDto dto); 
    Task DeleteAsync(int id); 
    Task<ICollection<Language>> GetAllAsync(); 
    Task<Language> GetByIdAsync(int id); 
    Task UpdateAsync(LanguageDto dto); 
    ICollection<Language> SortBy(ICollection<Language> languages, string sortFilter, bool descending = false);
}