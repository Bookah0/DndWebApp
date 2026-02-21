namespace Api.External.Languages;

public interface IExternalLanguageService
{
    Task FetchExternalLanguagesAsync(CancellationToken cancellationToken = default);
}