namespace Api.External.Alignments;

public interface IExternalAlignmentService
{
    Task FetchExternalAlignmentsAsync(CancellationToken cancellationToken = default);
}