namespace Api.Domain.Items.Models;

public class Vehicle : Item
{
    public required int? Speed { get; set; }
    public required string? SpeedUnit { get; set; }
    public required int? Capacity { get; set; }
    public required string? CapacityUnit { get; set; }
    public required bool Landborne { get; set; }
    public required bool Waterborne { get; set; }
}