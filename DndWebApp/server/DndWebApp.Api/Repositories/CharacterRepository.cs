using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;

namespace DndWebApp.Api.Repositories;

public class CharacterRepository(AppDbContext context) : EfRepository<Character>(context)
{
}