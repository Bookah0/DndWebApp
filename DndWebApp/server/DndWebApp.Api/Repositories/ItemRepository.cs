using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Items;

namespace DndWebApp.Api.Repositories;

public class ItemRepository(AppDbContext context) : EfRepository<Item>(context)
{
}

public class ArmorRepository(AppDbContext context) : EfRepository<Armor>(context)
{
}

public class WeaponRepository(AppDbContext context) : EfRepository<Weapon>(context)
{
}

public class ToolRepository(AppDbContext context) : EfRepository<Tool>(context)
{
}