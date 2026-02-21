using Api.Domain.Languages.Models;
using Api.Domain.Languages.DTOs;

namespace Api.Domain.Languages.Services;

public interface ILanguageService
{
    Task<Language> CreateAsync(CreateLanguageRequestDto dto); 
    Task DeleteAsync(int id); 
    Task<ICollection<Language>> GetAllAsync(); 
    Task<Language> GetByIdAsync(int id); 
    Task<Language> UpdateAsync(int id, UpdateLanguageRequestDto dto); 
    ICollection<Language> SortBy(ICollection<Language> languages, string sortFilter, bool descending = false);
}