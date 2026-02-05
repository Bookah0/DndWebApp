using Api.Models.Characters;
using Api.Models.DTOs;

namespace Api.Repositories.Interfaces;

public interface ISubraceRepository : IRepository<Subrace>
{
    Task<Subrace> GetWithAllDataAsync(int id);
    Task<Subrace> GetWithTraitsAsync(int id);
    Task<Subrace> GetByNameAsync(string name);
}