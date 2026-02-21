using Api.Domain.Shared.Interfaces;

namespace Api.Domain.Shared.Repositories;

public interface IChoiceRepository<T> : IRepository<T> where T : IFeatureChoice
{

}