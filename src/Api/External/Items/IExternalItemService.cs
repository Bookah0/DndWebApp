namespace Api.External.Items;

public interface IExternalItemService
{
    Task FetchExternalBasicItemsAsync(CancellationToken cancellationToken = default);
    Task FetchExternalMagicalItemsAsync(CancellationToken cancellationToken = default);
}