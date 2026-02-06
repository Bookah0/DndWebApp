namespace Api.Services.External.Implemented;

using System.Text.Json;
using Api.Middlewares.ExceptionHandling;
using Api.Models.DTOs.ExternalDTOs;
using Api.Models.Spells;
using Api.Models.Spells.Constants;
using Api.Repositories.Interfaces;
using Api.Services.External.Interfaces;
using Api.Services.Util;

public class ExternalSpellService(ISpellRepository repo, ILogger<ExternalSpellService> logger) : IExternalSpellService
{
    private readonly HttpClient client = new();

    public async Task FetchExternalSpellsAsync(CancellationToken cancellationToken = default)
    {
        if ((await repo.GetAllAsync()).Count > 0)
        {
            logger.LogInformation("Spells already exist in the database. Skipping fetch.");
            return;
        }

        logger.LogInformation("Fetching external spells.");

        var getOpenListResponse = await client.GetAsync("https://api.open5e.com/v1/spells/", cancellationToken);
        var resultOpen = await JsonSerializer.DeserializeAsync<EOpen5eResponseDto<EOpen5eSpellDto>>(getOpenListResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

        if (resultOpen is null || resultOpen.Count == 0)
        {
            throw new InvalidOperationException("No spells found in external APIs.");
        }

        logger.LogInformation("Fetched {SpellCount} spells from Open5e.", resultOpen.Count);

        foreach (var eOpenSpell in resultOpen.Results)
        {   
            var (range, rangeValue) = ParseSpellRange(eOpenSpell!.Range);
            var (castingTime, timeValue) = ParseCastingTime(eOpenSpell!.CastingTime);
            var (duration, durationValue) = ParseSpellDuration(eOpenSpell!.Duration);

            var spellTargeting = new SpellTargeting
            {
                TargetType = ParseTargetType(),
                Range = range,
                RangeValue = rangeValue,
                ShapeType = "",
            };

            List<string> spellTypes = [];
            if (eOpenSpell?.IsRitual == true)
            {
                spellTypes.Add(SpellType.Ritual);
            }
            if (duration != SpellDuration.Instantaneous)
            {
                spellTypes.Add(SpellType.Concentration);
            }
            if (castingTime == CastingTime.Reaction)
            {
                spellTypes.Add(SpellType.Reaction);
            }

            var castingRequirements = new CastingRequirements
            {
                Verbal = eOpenSpell?.RequiresVerbalComponents == true,
                Somatic = eOpenSpell?.RequiresSomaticComponents == true,
                Materials = eOpenSpell?.Material ?? null,
            };

            var eMagicSchool = eOpenSpell!.School;

            var spell = new Spell
            {
                Name = eOpenSpell!.Name,
                Description = eOpenSpell!.Description,
                Level = eOpenSpell!.SpellLevel,
                EffectsAtHigherLevels = eOpenSpell?.HigherLevels ?? "",
                Duration = duration,
                DurationValue = durationValue,
                CastingTime = castingTime,
                CastingTimeValue = timeValue,
                MagicSchool = ConstantsUtil.ResolveOptionOrThrow(eMagicSchool, MagicSchool.AllowedValues, "Magic School"),
                SpellTargeting = spellTargeting,
                SpellTypes = spellTypes,
                CastingRequirements = castingRequirements
            };

            await repo.CreateAsync(spell);

            /*
            Code for combining data from both APIs - currently disabled

            if (eOpenSpell is null)
                continue;

            var get5eResponse = await client.GetAsync($"https://www.dnd5eapi.co/api/2014/spells/{eOpenSpell.Index}", cancellationToken);
            var e5eSpell = await JsonSerializer.DeserializeAsync<EDnd5eApiSpellDto>(get5eResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken) 
                ?? throw new InvalidOperationException($"Failed to deserialize spell from https://www.dnd5eapi.co/api/2014/spells/{eOpenSpell.Index}");
            
            var (range, rangeValue) = e5eSpell != null ? ParseSpellRange(e5eSpell.Range) : ParseSpellRange(eOpenSpell!.Range);
            var (castingTime, timeValue) = e5eSpell != null ? ParseCastingTime(e5eSpell.CastingTime) : ParseCastingTime(eOpenSpell!.CastingTime);
            var (duration, durationValue) = e5eSpell != null ? ParseSpellDuration(e5eSpell.Duration) : ParseSpellDuration(eOpenSpell!.Duration);

            var spellTargeting = new SpellTargeting
            {
                TargetType = ParseTargetType(),
                Range = range,
                RangeValue = rangeValue,
                ShapeType = e5eSpell?.Aoe.AoeType ?? "",
            };

            List<string> spellTypes = [];
            if (e5eSpell?.Ritual == true || eOpenSpell?.IsRitual == true)
            {
                spellTypes.Add(SpellType.Ritual);
            }
            if (duration != SpellDuration.Instantaneous)
            {
                spellTypes.Add(SpellType.Concentration);
            }
            if (castingTime == CastingTime.Reaction)
            {
                spellTypes.Add(SpellType.Reaction);
            }

            var castingRequirements = new CastingRequirements
            {
                Verbal = e5eSpell?.Components.Contains("V") == true || eOpenSpell?.RequiresVerbalComponents == true,
                Somatic = e5eSpell?.Components.Contains("S") == true || eOpenSpell?.RequiresSomaticComponents == true,
                Materials = e5eSpell?.Material ?? eOpenSpell?.Material ?? null,
            };

            var eMagicSchool = e5eSpell != null ? e5eSpell.School.Name : eOpenSpell!.School;

            var spell = new Spell
            {
                Name = e5eSpell?.Name ?? eOpenSpell!.Name,
                Description = e5eSpell != null ? string.Join("\n", e5eSpell.Description) : eOpenSpell!.Description,
                Level = e5eSpell?.Level ?? eOpenSpell!.SpellLevel,
                EffectsAtHigherLevels = e5eSpell?.HigherLevel != null ? string.Join("\n", e5eSpell.HigherLevel) : eOpenSpell?.HigherLevels ?? "",
                Duration = duration,
                DurationValue = durationValue,
                CastingTime = castingTime,
                CastingTimeValue = timeValue,
                MagicSchool = ConstantsUtil.ResolveOptionOrThrow(eMagicSchool, MagicSchool.AllowedValues, "Magic School"),
                SpellTargeting = spellTargeting,
                SpellTypes = spellTypes,
                CastingRequirements = castingRequirements
            };

            await repo.CreateAsync(spell);*/
        }

        logger.LogInformation("Successfully fetched external spells. Count: {SpellCount}", resultOpen.Count);
    }

    private static (string, int) ParseCastingTime(string castingTimeStr)
    {
        var castingTimeMap = new (string pattern, string castingTime, int value)[]
        {
            ("1 action", CastingTime.Action, 1),
            ("1 bonus action", CastingTime.BonusAction, 1),
            ("1 reaction", CastingTime.Reaction, 1),
            ("1 minute", CastingTime.Minute, 1),
            ("5 minutes", CastingTime.Minute, 5),
            ("10 minutes", CastingTime.Minute, 10),
            ("1 hour", CastingTime.Hour, 1),
            ("8 hours", CastingTime.Hour, 8),
            ("24 hours", CastingTime.Hour, 24),
        };

        foreach (var (pattern, castingTime, value) in castingTimeMap)
        {
            if (castingTimeStr.StartsWith(pattern, StringComparison.CurrentCultureIgnoreCase))
                return (castingTime, value);
        }

        throw new ValidationException($"Casting time '{castingTimeStr}' not recognized.");
    }

    private static (string, int) ParseSpellDuration(string durationStr)
    {
        return durationStr.ToLower() switch
        {
            "instantaneous" => (SpellDuration.Instantaneous, 1),
            "concentration, up to 1 minute" => (SpellDuration.Minute, 1),
            "concentration, up to 10 minutes" => (SpellDuration.Minute, 10),
            "concentration, up to 1 hour" => (SpellDuration.Hour, 1),
            "concentration, up to 8 hours" => (SpellDuration.Hour, 8),
            "concentration, up to 24 hours" => (SpellDuration.Hour, 24),
            "up to 1 minute" => (SpellDuration.Minute, 1),
            "up to 10 minutes" => (SpellDuration.Minute, 10),
            "up to 1 hour" => (SpellDuration.Hour, 1),
            "up to 8 hours" => (SpellDuration.Hour, 8),
            "up to 24 hours" => (SpellDuration.Hour, 24),
            "1 minute" => (SpellDuration.Minute, 1),
            "5 minutes" => (SpellDuration.Minute, 5),
            "10 minutes" => (SpellDuration.Minute, 10),
            "1 hour" => (SpellDuration.Hour, 1),
            "8 hours" => (SpellDuration.Hour, 8),
            "24 hours" => (SpellDuration.Hour, 24),
            "up to 6 rounds" => (SpellDuration.Round, 6),
            "1 round" => (SpellDuration.Round, 1),
            "7 days" => (SpellDuration.Day, 7),
            "special" => (SpellDuration.Special, 0),
            "until dispelled" => (SpellDuration.UntilDispelled, 0),
            "permanent; one generation" => (SpellDuration.Permanent, 0),
            _ => throw new ValidationException($"Duration '{durationStr}' not recognized.")
        };
    }

    private static (string, int) ParseSpellRange(string rangeStr)
    {
        var split = rangeStr.Split(' ');
        if (int.TryParse(split[0], out int distance))
        {
            return split[1].ToLower() switch
            {
                "feet" => (SpellRange.Feet, distance),
                "mile" => (SpellRange.Mile, distance),
                _ => throw new ValidationException($"Range '{rangeStr}' not recognized.")
            };
        }
        return rangeStr.ToLower() switch
        {
            "self" => (SpellRange.Self, 0),
            "touch" => (SpellRange.Touch, 1),
            _ => throw new ValidationException($"Range '{rangeStr}' not recognized.")
        };
    }

    private static string ParseTargetType()
    {
        return ""; // Placeholder
    }
}

