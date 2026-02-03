using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.RequestDtos.Character;

namespace DndWebApp.Api.Repositories.Interfaces;

public interface IChoiceRepository<T> : IRepository<T> where T : IFeatureChoice
{

}