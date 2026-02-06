using Api.Models.Characters;
using Api.Models.DTOs.RequestDtos.Character;

namespace Api.Repositories.Interfaces;

public interface IChoiceRepository<T> : IRepository<T> where T : IFeatureChoice
{

}