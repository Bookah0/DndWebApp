using Api.Models.Characters;
using Api.Models.DTOs.ExternalDTOs;
using Api.Repositories.Interfaces;

namespace Api.Services.External.Interfaces;

public interface IExternalSpeciesService
{
    Task FetchExternalRacesAsync(CancellationToken cancellationToken = default);
    Task FetchExternalSubracesAsync(Race race, List<EIndexDto> subraceIndexList, CancellationToken cancellationToken = default);
}