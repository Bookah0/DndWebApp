using Api.Domain.Classes.Models;
using Api.External.Shared;

namespace Api.External.Classes;

public interface IExternalClassService
{
    Task FetchExternalClassesAsync(CancellationToken cancellationToken = default);
    Task FetchExternalSubclassesAsync(BaseClass clss, List<EIndexDto> subclassIndexList, CancellationToken cancellationToken = default);
    Task FetchExternalClassLevelsAsync(Class clss, CancellationToken cancellationToken = default);
    Task FetchExternalClassFeaturesAsync(CancellationToken cancellationToken = default);
}