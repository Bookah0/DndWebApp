using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.World;
using DndWebApp.Api.Utils;

namespace DndWebApp.Api.Models.Characters;


// Based on https://api.open5e.com/v2/backgrounds/
public class Background
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public bool IsHomebrew { get; set; } = false;

    public ICollection<PassiveEffect> BackgroundFeatures { get; set; } = [];
    public ICollection<Item> StartingItems { get; set; } = [];
    public ICollection<ItemChoice> StartingItemsOptions { get; set; } = [];
    public required Currency StartingCurrency { get; set; }
}
