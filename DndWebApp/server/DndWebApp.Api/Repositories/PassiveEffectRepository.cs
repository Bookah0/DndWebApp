using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;

namespace DndWebApp.Api.Repositories;

public class PassiveEffectRepository(AppDbContext context) : EfRepository<PassiveEffect>(context)
{
}