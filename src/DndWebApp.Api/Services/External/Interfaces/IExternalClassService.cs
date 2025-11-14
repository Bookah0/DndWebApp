using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.ExternalDtos;

namespace DndWebApp.Api.Services.External.Interfaces;

public interface IExternalClassService
{
    Task FetchExternalClassesAsync(CancellationToken cancellationToken = default);
    Task FetchExternalSubclassesAsync(Class clss, List<EIndexDto> subclassIndexList, CancellationToken cancellationToken = default);
    Task FetchExternalClassLevelsAsync(AClass clss, CancellationToken cancellationToken = default);
    Task FetchExternalClassFeaturesAsync(CancellationToken cancellationToken = default);
}