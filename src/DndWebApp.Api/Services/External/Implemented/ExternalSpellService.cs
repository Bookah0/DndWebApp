namespace DndWebApp.Api.Services.External.Implemented;

using System.Text.Json;
using DndWebApp.Api.Models.DTOs.ExternalDTOs;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Models.Spells.Constants;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.External.Interfaces;
using DndWebApp.Api.Services.Util;

public class ExternalSpellService : IExternalSpellService
{
    private readonly ISpellRepository repo;
    private readonly HttpClient client = new();

    public ExternalSpellService(ISpellRepository repo)
    {
        this.repo = repo;
    }

    public async Task FetchExternalSpellsAsync(CancellationToken cancellationToken = default)
    {
        if ((await repo.GetAllAsync()).Count > 0)
        {
            throw new InvalidOperationException("Spells already exist in the database. Skipping fetch.");
        }

        var getOpenListResponse = await client.GetAsync("https://api.open5e.com/v1/spells/", cancellationToken);
        var resultOpen = await JsonSerializer.DeserializeAsync<List<EOpen5eSpellDto>>(getOpenListResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

        if (resultOpen is null || resultOpen.Count == 0)
        {
            throw new InvalidOperationException("No spells found in external APIs.");
        }

        var seenIndexes = new HashSet<string>();

        foreach (var eOpenSpell in resultOpen)
        {
            if (seenIndexes.Contains(eOpenSpell.Index) || eOpenSpell is null)
            {
                continue;
            }
            seenIndexes.Add(eOpenSpell.Index);

            var get5eResponse = await client.GetAsync($"https://www.dnd5eapi.co/api/2014/spells/{eOpenSpell.Index}", cancellationToken);
            var e5eSpell = await JsonSerializer.DeserializeAsync<EDnd5eApiSpellDto>(get5eResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

            if (e5eSpell is null)
            {
                throw new InvalidOperationException($"Failed to deserialize spell from https://www.dnd5eapi.co/api/2014/spells/{eOpenSpell.Index}");
            }

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

            await repo.CreateAsync(spell);
        }
    }
        
    private static (string, int) ParseCastingTime(string castingTimeStr)
    {
        return castingTimeStr.ToLower() switch
        {
            "1 action" => (CastingTime.Action, 1),
            "1 bonus action" => (CastingTime.BonusAction, 1),
            "1 reaction" => (CastingTime.Reaction, 1),
            "1 minute" => (CastingTime.Minute, 1),
            "10 minutes" => (CastingTime.Minute, 10),
            "1 hour" => (CastingTime.Hour, 1),
            "8 hours" => (CastingTime.Hour, 8),
            "12 hours" => (CastingTime.Hour, 12),
            "24 hours" => (CastingTime.Hour, 24),
            _ => throw new ArgumentException($"Casting time '{castingTimeStr}' not recognized.")
        };
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
            "special" => (SpellDuration.Special, 0),
            _ => throw new ArgumentException($"Duration '{durationStr}' not recognized.")
        };
    }

    private static (string, int) ParseSpellRange(string rangeStr)
    {
        return rangeStr.ToLower() switch
        {
            "self" => (SpellRange.Self, 0),
            "touch" => (SpellRange.Touch, 1),
            "30 feet" => (SpellRange.Feet, 30),
            "60 feet" => (SpellRange.Feet, 60),
            "90 feet" => (SpellRange.Feet, 90),
            "120 feet" => (SpellRange.Feet, 120),
            "150 feet" => (SpellRange.Feet, 150),
            "300 feet" => (SpellRange.Feet, 300),
            "500 feet" => (SpellRange.Feet, 500),
            "1 mile" => (SpellRange.Mile, 1),
            _ => throw new ArgumentException($"Range '{rangeStr}' not recognized.")
        };
    }

    private static string ParseTargetType()
    {
        return ""; // Placeholder
    }
}

