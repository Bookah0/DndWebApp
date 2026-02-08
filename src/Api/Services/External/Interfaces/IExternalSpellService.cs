using Api.Models.Characters;
using Api.Repositories.Interfaces;

namespace Api.Services.External.Interfaces;

public interface IExternalSpellService
{
    Task FetchExternalSpellsAsync(CancellationToken cancellationToken = default);
}