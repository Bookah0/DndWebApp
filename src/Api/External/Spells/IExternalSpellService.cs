namespace Api.External.Spells;

public interface IExternalSpellService
{
    Task FetchExternalSpellsAsync(CancellationToken cancellationToken = default);
}