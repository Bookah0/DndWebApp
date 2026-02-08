namespace Api.Services.External.Interfaces;

public interface IExternalItemService
{
    Task FetchExternalBasicItemsAsync(CancellationToken cancellationToken = default);
    Task FetchExternalMagicalItemsAsync(CancellationToken cancellationToken = default);
}