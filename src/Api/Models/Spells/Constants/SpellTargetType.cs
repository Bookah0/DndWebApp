namespace Api.Models.Spells.Constants;

public static class SpellTargetType
{
    public const string Creature = "Creature";
    public const string Object = "Object";
    public const string Area = "Area";
    public const string Self = "Self";
    public const string Point = "Point";
    public const string Cylinder = "Cylinder";
    public const string Cone = "Cone";
    public const string Line = "Line";

    public static readonly IReadOnlySet<string> AllowedValues = new HashSet<string>
    {
        Creature,
        Object,
        Area,
        Self,
        Point,
        Cylinder,
        Cone,
        Line
    };
}