namespace DndWebApp.Api.Services.External.Interfaces;

public interface IExternalSkillService
{
    Task FetchExternalSkillsAsync(CancellationToken cancellationToken = default);
}