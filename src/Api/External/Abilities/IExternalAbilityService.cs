namespace Api.External.Abilities;

public interface IExternalAbilityService
{
    Task FetchExternalAbilitiesAsync(CancellationToken cancellationToken = default);
}