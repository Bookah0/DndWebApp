using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;

namespace DndWebApp.Api.Repositories;

public class FeatRepository : EfRepository<Feat>
{
    public FeatRepository(AppDbContext context) : base(context)
    {
    }
}