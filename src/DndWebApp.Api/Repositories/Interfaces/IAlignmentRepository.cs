using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.World;
using DndWebApp.Api.Models.World.Enums;

namespace DndWebApp.Api.Repositories.Interfaces;

public interface IAlignmentRepository : IRepository<Alignment>
{
    Task<Alignment?> GetByTypeAsync(AlignmentType type);
}