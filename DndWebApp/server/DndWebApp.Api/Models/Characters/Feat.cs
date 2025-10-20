
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Models.World;
using DndWebApp.Api.Utils;

namespace DndWebApp.Api.Models.Characters;

// Based on https://api.open5e.com/v2/feats/
public class Feat : PassiveEffect
{
    public string Prerequisite { get; set; } = "";
    
}