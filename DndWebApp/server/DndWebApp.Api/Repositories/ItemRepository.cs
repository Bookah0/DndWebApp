using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Items;

namespace DndWebApp.Api.Repositories;

public class ItemRepository(AppDbContext context) : EfRepository<Item>(context)
{
}