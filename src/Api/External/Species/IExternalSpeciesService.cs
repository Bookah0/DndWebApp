using Api.Domain.Species.Models;
using Api.External.Shared;

namespace Api.External.Species;

public interface IExternalSpeciesService
{
    Task FetchExternalRacesAsync(CancellationToken cancellationToken = default);
    Task FetchExternalSubracesAsync(Race race, List<EIndexDto> subraceIndexList, CancellationToken cancellationToken = default);
}