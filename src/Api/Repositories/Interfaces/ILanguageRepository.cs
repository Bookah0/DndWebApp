using Api.Models.Characters;
using Api.Models.DTOs;
using Api.Models.World;

namespace Api.Repositories.Interfaces;

public interface ILanguageRepository : IRepository<Language>
{
    Task<Language> GetByNameAsync(string name);
}