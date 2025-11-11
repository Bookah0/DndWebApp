namespace DndWebApp.Api.Services.External.Interfaces;

public interface IExternalAlignmentService
{
    Task FetchExternalAlignmentsAsync(CancellationToken cancellationToken = default);
}