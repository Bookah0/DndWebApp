using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.World;

namespace DndWebApp.Api.Models.Characters;


// Based on https://api.open5e.com/v2/backgrounds/
public class Background
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public bool IsHomebrew { get; set; } = false;

    public ICollection<Feature> Features { get; set; } = [];
    public ICollection<Item> StartingItems { get; set; } = [];
    public ICollection<ItemChoice> StartingItemsOptions { get; set; } = [];
    public required Currency StartingCurrency { get; set; }
}
