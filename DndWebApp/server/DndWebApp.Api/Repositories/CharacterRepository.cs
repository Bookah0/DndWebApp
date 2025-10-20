using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;

namespace DndWebApp.Api.Repositories;

public class CharacterRepository : EfRepository<Character>
{
    public CharacterRepository(AppDbContext context) : base(context)
    {
    }
}