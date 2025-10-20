using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories;

public class SkillRepository : EfRepository<Skill>
{
    public SkillRepository(AppDbContext context) : base(context)
    {
    }
}