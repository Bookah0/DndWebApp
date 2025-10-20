using DndWebApp.Api.Data;
using DndWebApp.Api.Models.World;

namespace DndWebApp.Api.Repositories;

public class AlignmentRepository(AppDbContext context) : EfRepository<Alignment>(context)
{
}