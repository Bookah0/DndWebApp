using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;

namespace DndWebApp.Api.Repositories;

public class ClassRepository(AppDbContext context) : EfRepository<Class>(context)
{
}