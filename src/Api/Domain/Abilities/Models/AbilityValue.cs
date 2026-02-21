namespace Api.Domain.Abilities.Models;

public class AbilityValue
{
    public int Id { get; set; }
    public required int AbilityId { get; set; }
    public required Ability Ability { get; set; }
    public required int Value { get; set; }
}