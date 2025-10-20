using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;

namespace DndWebApp.Api.Repositories;

public class ClassRepository : EfRepository<Class>
{
    public ClassRepository(AppDbContext context) : base(context)
    {
    }
}