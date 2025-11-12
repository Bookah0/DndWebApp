using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Items;

namespace DndWebApp.Api.Repositories.Interfaces;

public interface IToolRepository : IRepository<Tool>
{
    Task<Tool?> GetWithAllDataAsync(int id);
}