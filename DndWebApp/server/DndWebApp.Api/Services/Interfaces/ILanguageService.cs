using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.World;
using DndWebApp.Api.Services.Enums;

namespace DndWebApp.Api.Services.Interfaces;

public interface ILanguageService
{
    Task<Language> CreateAsync(LanguageDto dto); 
    Task DeleteAsync(int id); 
    Task<ICollection<Language>> GetAllAsync(); 
    Task<Language> GetByIdAsync(int id); 
    Task UpdateAsync(LanguageDto dto); 
    ICollection<Language> SortBy(ICollection<Language> languages, LanguageSortFilter sortFilter, bool descending = false);
}