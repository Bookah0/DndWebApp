using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories;

public class ItemRepository : EfRepository<Item>
{
    public ItemRepository(AppDbContext context) : base(context)
    {
    }
}