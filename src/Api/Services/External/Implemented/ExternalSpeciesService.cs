namespace Api.Services.External.Implemented;

using System.Text.Json;
using Api.Models.Characters;
using Api.Models.DTOs.ExternalDTOs;
using Api.Models.Features;
using Api.Repositories.Interfaces;
using Api.Services.External.Interfaces;
using Api.Validation.AllowedValues;
using static Api.Validation.AllowedValues.ValuesValidator;

public class ExternalSpeciesService(IRaceRepository raceRepo, ISubraceRepository subraceRepo, IAbilityRepository abilityRepo, ILogger<ExternalSpeciesService> logger) : IExternalSpeciesService
{
    private readonly HttpClient client = new();

    public async Task FetchExternalRacesAsync(CancellationToken cancellationToken = default)
    {
        if ((await raceRepo.GetAllAsync()).Count > 0)
        {
            logger.LogInformation("Races already exist in the database. Skipping fetch.");
            return;
        }

        logger.LogInformation("Fetching external races.");

        var getListResponse = await client.GetAsync("https://www.dnd5eapi.co/api/2014/races/", cancellationToken);
        var result = await JsonSerializer.DeserializeAsync<EIndexListDto>(getListResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

        if (result is null || result.Results.Count == 0)
        {
            throw new InvalidOperationException("No races found in external API.");
        }

        foreach (var item in result.Results)
        {
            var getResponse = await client.GetAsync($"https://www.dnd5eapi.co/api/2014/races/{item.Index}", cancellationToken);
            var eRace = await JsonSerializer.DeserializeAsync<ERaceDto>(getResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

            if (eRace is null)
            {
                throw new InvalidOperationException($"Failed to deserialize race {item.Index}.");
            }

            var description = new SpeciesInfo
            {
                General = "",
                Aging = eRace.Age,
                CommonAlignment = eRace.PreferedAlignment,
                Size = eRace.SizeDescription,
                Languages = eRace.AvailableLanguagesDescription
            };

            var race = new Race
            {
                Name = eRace.Name,
                Speed = eRace.Speed,
                Info = description,
                Size = ResolveValueOrThrow<CreatureSize>(eRace.Size),
                Traits = [],
                SubRaces = [],

                CreatedAt = DateTime.UtcNow,
                CreatedBy = null,
                IsHomebrew = false,
                IsPublic = true,
                CloningAllowed = true
            };

            await raceRepo.CreateAsync(race);

            await AddAbilityScoreBonusesAsTraitAsync(eRace, race, raceRepo);
            ParseTraits(race);
            await FetchExternalSubracesAsync(race, eRace.Subraces, cancellationToken);
        }

        logger.LogInformation("Successfully fetched external races. Count: {RaceCount}", result.Results.Count);
    }

    // TODO: For subraces, switch to an API with better data coverage
    public async Task FetchExternalSubracesAsync(Race race, List<EIndexDto> subraceIndexList, CancellationToken cancellationToken = default)
    {
        if (subraceIndexList.Count == 0)
            return;

        logger.LogInformation("Fetching external subraces for race, Name: {RaceName}, SubraceCount: {SubraceCount}", race.Name, subraceIndexList.Count);

        foreach (var item in subraceIndexList)
        {
            var getResponse = await client.GetAsync($"https://www.dnd5eapi.co/api/2014/subraces/{item.Index}", cancellationToken);
            var eSubrace = await JsonSerializer.DeserializeAsync<ESubraceDto>(getResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

            if (eSubrace is null)
            {
                throw new InvalidOperationException($"Failed to deserialize subrace {item.Index}.");
            }
            if (await subraceRepo.GetByNameAsync(eSubrace.Name) is not null)
            {
                throw new InvalidOperationException($"Subrace {eSubrace.Name} already exists. Skipping.");
            }

            var description = new SpeciesInfo
            {
                General = eSubrace.Description
            };

            var subrace = new Subrace
            {
                Name = eSubrace.Name,
                Speed = race.Speed,
                Info = description,
                Size = race.Size,
                Traits = [],
                ParentRace = race,
                ParentRaceId = race.Id,

                CreatedAt = DateTime.UtcNow,
                CreatedBy = null,
                IsHomebrew = false,
                IsPublic = true,
                CloningAllowed = true
            };

            await subraceRepo.CreateAsync(subrace);
            await AddAbilityScoreBonusesAsTraitAsync(eSubrace, subrace, subraceRepo);
            ParseTraits(subrace);
        }

        logger.LogInformation("Successfully fetched external subraces for race, Name: {RaceName}, SubraceCount: {SubraceCount}", race.Name, subraceIndexList.Count);
    }

    /// <summary>
    /// Helper method that converts ability score bonuses from the external species DTO into a Trait which is added to the given species entity. 
    /// Made generic to work for both Race and Subrace.
    /// </summary>
    /// <typeparam name="T">Either a Race or Subrace</typeparam>
    /// <param name="eSpecies">The dto representing the species entity as fetched fom the api</param>
    /// <param name="species">The race or subrace that is saved in the database</param>
    /// <param name="speciesRepo">Which repository to call UpdateAsync() on when done</param>
    private async Task AddAbilityScoreBonusesAsTraitAsync<T>(ESpeciesDto eSpecies, T species, IRepository<T> speciesRepo) where T : Species
    {
        var abilityIncreases = new List<AbilityValue>();
        var traitDescription = $"Being a {eSpecies.Name} has increased your ability scores:";

        foreach (var abilityIncrease in eSpecies.AbilityBonuses)
        {
            var ability = await abilityRepo.GetByShortNameAsync(abilityIncrease.AbilityScore.Name);

            abilityIncreases.Add(new AbilityValue
            {
                AbilityId = ability.Id,
                Value = abilityIncrease.Bonus,
                Ability = ability
            });

            if (eSpecies.AbilityBonuses.Count == 1)
            {
                traitDescription += $"{ability.FullName}: +{abilityIncrease.Bonus}";
            }
            else
            {
                traitDescription += $"\n- {ability.FullName}: +{abilityIncrease.Bonus}";
            }
        }

        var increaseTrait = new Trait
        {
            Name = $"Ability Score Increases",
            Description = traitDescription,
            FromRace = species,
            RaceId = species.Id,
            AbilityIncreases = abilityIncreases,

            CreatedAt = DateTime.UtcNow,
            CreatedBy = null,
            IsHomebrew = false,
            IsPublic = true,
            CloningAllowed = true
        };

        species.Traits.Add(increaseTrait);
        await speciesRepo.UpdateAsync(species);
    }

    // TODO: Parses Traits
    private static void ParseTraits(Species species)
    {

    }
}