using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Spells;

namespace DndWebApp.Api.Repositories;

public class SpellRepository(AppDbContext context) : EfRepository<Spell>(context)
{
}