using Api.Domain.Characters.Models;
using Api.Domain.Items.Models;
using Api.Domain.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Backgrounds.Models;


// Based on https://api.open5e.com/v2/backgrounds/
public class Background : CreatableEntity
{
    public required string Name { get; set; }
    public required string Description { get; set; }

    public ICollection<BackgroundFeature> Features { get; set; } = [];
    public ICollection<Item> StartingItems { get; set; } = [];
    public ICollection<StartingItemOption> StartingItemsOptions { get; set; } = [];
    public required Currency StartingCurrency { get; set; }
}

[Owned]
public class StartingItemOption
{
    public int Id { get; set; }
    public required string Description { get; set; }
    public required ICollection<int> ItemOptionIds { get; set; }
}
