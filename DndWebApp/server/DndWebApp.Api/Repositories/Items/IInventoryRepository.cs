using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Items;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Items;

public interface IInventoryRepository : IRepository<Inventory>
{
    /// <summary>
    /// Retrieves a <see cref="Inventory"/> entity by its <paramref name="id"/>, 
    /// including the navigation properties related to stored items
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Inventory"/> to retrieve.</param>
    /// <returns>
    /// The <see cref="Inventory"/> entity with the navigation properties related to stored items,
    /// or <c>null</c> if no race with the specified <paramref name="id"/> exists.
    /// </returns>
    /// <remarks>
    /// Typically used for detailed display of all items in the inventory of a character, excluding what is and is not equipped.
    /// </remarks>
    Task<Inventory?> GetWithStoredItemsAsync(int id);
}