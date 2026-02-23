using Api.Domain.Languages.DTOs;
using Api.Domain.Languages.Models;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Repositories;

namespace Api.Domain.Languages.Repositories;

public interface ILanguageRepository : IRepository<Language>
{
    Task<Language> GetByNameAsync(string name);
    Task<(int, ICollection<Language>)> GetFilteredAsync(LanguageFilterDto filter, PaginationRequestDto pagination);
}