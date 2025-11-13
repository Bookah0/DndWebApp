using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.World;
using DndWebApp.Api.Models.World.Enums;

namespace DndWebApp.Api.Repositories.Interfaces;

public interface ILanguageRepository : IRepository<Language>
{
    Task<Language?> GetByTypeAsync(LanguageType type);
}