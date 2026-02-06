using Api.Data;
using Api.Models.Items;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Interfaces;

public interface IInventoryRepository : IRepository<Inventory>
{
    Task<Inventory> GetWithStoredItemsAsync(int id);
}