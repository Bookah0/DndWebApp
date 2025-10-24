using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Items;

namespace DndWebApp.Api.Repositories.Items;

public interface IItemRepository : IRepository<Item>
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
    new Task<Item?> GetByIdAsync(int id);

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
    new Task<ICollection<Item>> GetAllAsync();
}