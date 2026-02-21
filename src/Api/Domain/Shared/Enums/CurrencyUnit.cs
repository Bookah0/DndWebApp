namespace Api.Domain.Shared.Enums;

public class CurrencyUnit : IValuesProvider
{
    public const string Copper = "CP";
    public const string Silver = "SP";
    public const string Electrum = "EP";
    public const string Gold = "GP";
    public const string Platinum = "PP";
    
    public static readonly IReadOnlySet<string> AllowedUnits = new HashSet<string>
    {
        Copper, Silver, Electrum, Gold, Platinum
    };

    static IReadOnlySet<string> IValuesProvider.Values => AllowedUnits;
}