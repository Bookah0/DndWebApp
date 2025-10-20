using DndWebApp.Api.Data;
using DndWebApp.Api.Models.World;

namespace DndWebApp.Api.Repositories;

public class LanguageRepository(AppDbContext context) : EfRepository<Language>(context)
{
}