namespace DndWebApp.Api.Services.External.Interfaces;

public interface IExternalFeatService
{
    Task FetchExternalFeatsAsync(CancellationToken cancellationToken = default);
}