using Api.Models.Items;

namespace Api.Services.Util;

public static class CurrencyUtil
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

    public static void ConvertCurrency(Currency currency)
    {
        var valueInBrass = currency.Brass + (currency.Copper * 10) + (currency.Silver * 100) + (currency.Gold * 1000) + (currency.Electrum * 10000);

        currency.Electrum = valueInBrass / 10000;
        currency.Gold = valueInBrass % 10000 / 1000;
        currency.Silver = valueInBrass % 1000 / 100;
        currency.Copper = valueInBrass % 100 / 10;
        currency.Brass = valueInBrass % 10;
    }
}