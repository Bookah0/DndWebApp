using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.ExternalDtos;
using DndWebApp.Api.Repositories.Interfaces;

namespace DndWebApp.Api.Services.External.Interfaces;

public interface IExternalSpeciesService
{
    Task FetchExternalRacesAsync(CancellationToken cancellationToken = default);
    Task FetchExternalSubracesAsync(Race race, List<EIndexDto> subraceIndexList, CancellationToken cancellationToken = default);
}