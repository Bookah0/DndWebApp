namespace DndWebApp.Api.Services.External.Interfaces;

public interface IExternalAbilityService
{
    Task FetchExternalAbilitiesAsync(CancellationToken cancellationToken = default);
}