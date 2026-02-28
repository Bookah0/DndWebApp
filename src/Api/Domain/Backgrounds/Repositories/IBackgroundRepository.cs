using Api.Domain.Backgrounds.DTOs;
using Api.Domain.Backgrounds.Models;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Repositories;

namespace Api.Domain.Backgrounds.Repositories;

public interface IBackgroundRepository : IRepository<Background>
{
    Task<Background> GetWithFeaturesAsync(int id);
    Task<Background> GetWithAllDataAsync(int id);
    Task<ICollection<Background>> GetAllWithAllDataAsync();
	Task<ICollection<Background>> GetAllAsync(BackgroundFilterDto? filter = null, PaginationRequestDto? pagination = null);
}