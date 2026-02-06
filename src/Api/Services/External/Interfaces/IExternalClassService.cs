using Api.Models.Characters;
using Api.Models.DTOs.ExternalDTOs;

namespace Api.Services.External.Interfaces;

public interface IExternalClassService
{
    Task FetchExternalClassesAsync(CancellationToken cancellationToken = default);
    Task FetchExternalSubclassesAsync(Class clss, List<EIndexDto> subclassIndexList, CancellationToken cancellationToken = default);
    Task FetchExternalClassLevelsAsync(AClass clss, CancellationToken cancellationToken = default);
    Task FetchExternalClassFeaturesAsync(CancellationToken cancellationToken = default);
}