
namespace DndWebApp.Api.Models.Characters;

// Based on https://5e-bits.github.io/docs/api/
public class Trait : CharacterFeature
{
    public required ICollection<Race> FromRaces { get; set; }
}