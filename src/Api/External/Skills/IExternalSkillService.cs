namespace Api.External.Skills;

public interface IExternalSkillService
{
    Task FetchExternalSkillsAsync(CancellationToken cancellationToken = default);
}