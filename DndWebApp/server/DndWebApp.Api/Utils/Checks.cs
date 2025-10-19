using DndWebApp.Api.Models.Characters;

namespace DndWebApp.Api.Utils;

public class DCCheck
{
    public required int DC { get; set; }
    public required Ability Ability { get; set; }
    public string Description { get; set; } = "";
    public bool Advantage { get; set; } = false;
}