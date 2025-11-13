namespace DndWebApp.Api.Services.External.Interfaces;

public interface IExternalClassService
{
    Task FetchExternalClassesAsync(CancellationToken cancellationToken = default);
    Task FetchExternalSubclassesAsync(CancellationToken cancellationToken = default);
    Task FetchExternalClassLevelsAsync(CancellationToken cancellationToken = default);
    Task FetchExternalClassFeaturesAsync(CancellationToken cancellationToken = default);
}