using Api.Models.Characters;
using Api.Models.DTOs.ExternalDTOs;

namespace Api.Services.External.Interfaces;

public interface IExternalClassService
{
    Task FetchExternalClassesAsync(CancellationToken cancellationToken = default);
    Task FetchExternalSubclassesAsync(BaseClass clss, List<EIndexDto> subclassIndexList, CancellationToken cancellationToken = default);
    Task FetchExternalClassLevelsAsync(Class clss, CancellationToken cancellationToken = default);
    Task FetchExternalClassFeaturesAsync(CancellationToken cancellationToken = default);
}