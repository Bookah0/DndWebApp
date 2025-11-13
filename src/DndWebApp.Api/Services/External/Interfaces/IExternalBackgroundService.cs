namespace DndWebApp.Api.Services.External.Interfaces;

public interface IExternalBackgroundService
{
    Task FetchExternalBackgroundsAsync(CancellationToken cancellationToken = default);
}