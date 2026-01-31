
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.Character;
using DndWebApp.Api.Models.DTOs.ResponseDtos;

namespace DndWebApp.Api.Services.Interfaces;

public interface ILanguageService
{
    Task<LanguageResponseDto> CreateAsync(LanguageDto dto); 
    Task DeleteAsync(int id); 
    Task<ICollection<LanguageResponseDto>> GetAllAsync(); 
    Task<LanguageResponseDto> GetByIdAsync(int id); 
    Task UpdateAsync(int id, LanguageDto dto); 
    ICollection<Language> SortBy(ICollection<Language> languages, string sortFilter, bool descending = false);
}