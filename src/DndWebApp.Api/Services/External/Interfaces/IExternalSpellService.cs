using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Repositories.Interfaces;

namespace DndWebApp.Api.Services.External.Interfaces;

public interface IExternalSpellService
{
    Task FetchExternalSpellsAsync(CancellationToken cancellationToken = default);
}