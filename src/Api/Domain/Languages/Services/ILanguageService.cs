using Api.Domain.Languages.Models;
using Api.Domain.Languages.DTOs;
using Api.Domain.Shared.DTOs;

namespace Api.Domain.Languages.Services;

public interface ILanguageService
{
    Task<Language> CreateAsync(CreateLanguageRequestDto dto); 
    Task DeleteAsync(int id); 
    Task<ICollection<Language>> GetAllAsync(); 
    Task<Language> GetByIdAsync(int id); 
    Task<Language> UpdateAsync(int id, UpdateLanguageRequestDto dto); 
    Task<(int, ICollection<Language>)> GetFilteredAsync(LanguageFilterDto filter, PaginationRequestDto pagination);
}