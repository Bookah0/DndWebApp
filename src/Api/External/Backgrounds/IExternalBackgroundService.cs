namespace Api.External.Backgrounds;

public interface IExternalBackgroundService
{
    Task FetchExternalBackgroundsAsync(CancellationToken cancellationToken = default);
}