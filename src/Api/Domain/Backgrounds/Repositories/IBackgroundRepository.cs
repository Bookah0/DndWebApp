using Api.Domain.Backgrounds.Models;
using Api.Domain.Shared.Repositories;

namespace Api.Domain.Backgrounds.Repositories;

public interface IBackgroundRepository : IRepository<Background>
{
    Task<Background> GetWithFeaturesAsync(int id);
    Task<Background> GetWithAllDataAsync(int id);
    Task<ICollection<Background>> GetAllWithAllDataAsync();
}