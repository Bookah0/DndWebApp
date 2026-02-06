using Api.Models.Characters;
using Api.Models.DTOs;
using Api.Models.Items;

namespace Api.Repositories.Interfaces;

public interface IToolRepository : IRepository<Tool>
{
    Task<Tool> GetWithAllDataAsync(int id);
}