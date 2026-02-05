namespace DndWebApp.Api.Services.External.Interfaces;

public interface IExternalLanguageService
{
    Task FetchExternalLanguagesAsync(CancellationToken cancellationToken = default);
}