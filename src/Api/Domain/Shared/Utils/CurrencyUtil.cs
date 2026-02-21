using Api.Domain.Characters.Models;

namespace Api.Domain.Shared.Utils;

public static class CurrencyUtil
{
    public static Currency ConvertCurrency(Currency currency)
    {
        var valueInBrass = currency.Brass + (currency.Copper * 10) + (currency.Silver * 100) + (currency.Gold * 1000) + (currency.Electrum * 10000);

        currency.Electrum = valueInBrass / 10000;
        currency.Gold = valueInBrass % 10000 / 1000;
        currency.Silver = valueInBrass % 1000 / 100;
        currency.Copper = valueInBrass % 100 / 10;
        currency.Brass = valueInBrass % 10;
        return currency;
    }
}