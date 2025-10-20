using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;

namespace DndWebApp.Api.Repositories;

public class SkillRepository(AppDbContext context) : EfRepository<Skill>(context)
{
}