using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.World;
using DndWebApp.Api.Utils;

namespace DndWebApp.Api.Models.Characters;


// Based on https://api.open5e.com/v2/backgrounds/
public class Background : BenefitProvider
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public bool IsHomebrew { get; set; } = false;

    public List<Item> StartingItems { get; set; } = [];
    public List<ChoiceOption<Item>> StartingItemsOptions { get; set; } = [];
    public List<Feature> BackgroundFeatures { get; set; } = [];
}