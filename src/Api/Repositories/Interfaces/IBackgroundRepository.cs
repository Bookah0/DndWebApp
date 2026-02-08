using Api.Models.Characters;
using Api.Models.DTOs;

namespace Api.Repositories.Interfaces;

public interface IBackgroundRepository : IRepository<Background>
{
    Task<Background> GetWithFeaturesAsync(int id);
    Task<Background> GetWithAllDataAsync(int id);
}