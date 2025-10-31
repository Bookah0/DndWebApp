using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.World;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Models.Characters;


// Based on https://api.open5e.com/v2/backgrounds/
public class Background
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public bool IsHomebrew { get; set; } = false;

    public ICollection<AFeature> Features { get; set; } = [];
    public ICollection<Item> StartingItems { get; set; } = [];
    public ICollection<StartingItemOption> StartingItemsOptions { get; set; } = [];
    public required Currency StartingCurrency { get; set; }
}

[Owned]
public class StartingItemOption
{
    public required string Description { get; set; }
    public required ICollection<int> ItemOptionIds { get; set; }
}
