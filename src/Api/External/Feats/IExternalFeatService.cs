namespace Api.External.Feats;

public interface IExternalFeatService
{
    Task FetchExternalFeatsAsync(CancellationToken cancellationToken = default);
}