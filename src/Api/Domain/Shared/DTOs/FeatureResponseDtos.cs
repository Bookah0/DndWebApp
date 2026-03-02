namespace Api.Domain.Shared.DTOs;

public abstract class FeatureResponseDto : CreatableEntityResponseDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
}

public class FeatResponseDto : FeatureResponseDto
{
    public required string Prerequisite { get; set; }
    public required int FromClassId { get; set; }
    public required int FromRaceId { get; set; }
    public required int FromBackgroundId { get; set; }
}

public class BackgroundFeatureResponseDto : FeatureResponseDto
{
    public required int BackgroundId { get; set; }
}

public class ClassFeatureResponseDto : FeatureResponseDto
{
    public required int LevelId { get; set; }
    public required int ClassId { get; set; }
}

public class TraitResponseDto : FeatureResponseDto
{
    public required int RaceId { get; set; }
}
