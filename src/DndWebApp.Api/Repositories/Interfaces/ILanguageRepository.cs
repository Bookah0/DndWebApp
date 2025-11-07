using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.World;

namespace DndWebApp.Api.Repositories.Interfaces;

public interface ILanguageRepository : IRepository<Language>
{
    Task<Language?> GetByNameAsync(string name);
}