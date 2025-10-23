using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Items;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories;

public class ItemRepository(AppDbContext context) : EfRepository<Item>(context)
{
    /// <summary>
    /// Retrieves a general <see cref="Item"/> entity by its <paramref name="id"/>,
    /// excluding any specialized derived types: <see cref="Weapon"/>, <see cref="Armor"/>, and <see cref="Tool"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Item"/> to retrieve.</param>
    /// <returns>
    /// The <see cref="Item"/> entity if it exists and is not a <see cref="Weapon"/>, <see cref="Armor"/>, or <see cref="Tool"/>;
    /// otherwise, <c>null</c>.
    /// </returns>
    /// <remarks>
    /// This method is intended for retrieving only "general" items that are not specialized into armor, weapons, or tools.
    /// This override is necessary because <see cref="Weapon"/>, <see cref="Armor"/>, and <see cref="Tool"/> entities
    /// are managed and retrieved through their own dedicated repositories.
    /// Typical use cases include listing or displaying such general items in inventories, item selections and shops.
    /// </remarks>
    public override async Task<Item?> GetByIdAsync(int id)
    {
        return await dbSet
            .Where(i => i.Id == id && !(i is Weapon) && !(i is Armor) && !(i is Tool))
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Retrieves all <see cref="Item"/> entities that are not <see cref="Weapon"/>, <see cref="Armor"/>, or <see cref="Tool"/>.
    /// </summary>
    /// <returns>
    /// A collection of <see cref="Item"/> entities excluding <see cref="Weapon"/>, <see cref="Armor"/>, and <see cref="Tool"/> types.
    /// </returns>
    /// <remarks>
    /// This override is required because <see cref="Weapon"/>, <see cref="Armor"/>, and <see cref="Tool"/> entities
    /// are managed and retrieved through their respective repositories.
    /// </remarks>
    public override async Task<ICollection<Item>> GetAllAsync()
    {
        return await dbSet
            .Where(i => !(i is Weapon) && !(i is Armor) && !(i is Tool))
            .ToListAsync();
    }
}

/// <summary>
/// Repository for managing <see cref="Armor"/> entities.
/// </summary>
/// <remarks>
/// Currently empty as all basic CRUD operations are handled by <see cref="EfRepository{T}"/>.
/// Dedicated repository exists for consistency with other item types and future flexibility.
/// </remarks>
public class ArmorRepository(AppDbContext context) : EfRepository<Armor>(context)
{

}

/// <summary>
/// Repository for managing <see cref="Weapon"/> entities.
/// </summary>
/// <remarks>
/// Currently empty as all basic CRUD operations are handled by <see cref="EfRepository{T}"/>.
/// Dedicated repository exists for consistency with other item types and future flexibility.
/// </remarks>
public class WeaponRepository(AppDbContext context) : EfRepository<Weapon>(context)
{
    
}

public class ToolRepository(AppDbContext context) : EfRepository<Tool>(context)
{
    /// <summary>
    /// Retrieves an <see cref="Tool"/> entity by its <paramref name="id"/>, 
    /// including related navigation properties: <see cref="Tool.Properties"/> and <see cref="Tool.Activities"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Tool"/> to retrieve.</param>
    /// <returns>
    /// The <see cref="Tool"/> entity with all related activities and properties,
    /// or <c>null</c> if no ability with the specified <paramref name="id"/> exists.
    /// </returns>
    /// <remarks>
    /// Typical use cases include detailed display of a single <see cref="Tool"/> with its activities and properties.
    /// </remarks>
    public async Task<Tool?> GetWithAllDataAsync(int id)
    {
        return await dbSet
            .Include(t => t.Properties)
            .Include(t => t.Activities)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <summary>
    /// Retrieves all <see cref="Tool"/> entities, 
    /// including related navigation properties: <see cref="Tool.Properties"/> and <see cref="Tool.Activities"/>.
    /// </summary>
    /// <returns>
    /// The <see cref="Tool"/> entities with all related activities and properties.
    /// </returns>
    /// <remarks>
    /// Typical use cases include displaying all <see cref="Tool"/>s alongside their related activities and properties.
    /// </remarks>
    public async Task<ICollection<Tool>> GetAllWithAllDataAsync()
    {
        return await dbSet
            .Include(t => t.Properties)
            .Include(t => t.Activities)
            .ToListAsync();
    }
}