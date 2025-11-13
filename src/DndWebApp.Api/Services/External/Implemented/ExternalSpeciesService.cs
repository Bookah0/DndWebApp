using System.Text.Json;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Characters.Enums;
using DndWebApp.Api.Models.DTOs.ExternalDtos;
using DndWebApp.Api.Models.ExternalDTOs;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.External.Interfaces;
using DndWebApp.Api.Services.Util;

public class ExternalSpeciesService : IExternalSpeciesService
{
    private readonly IRaceRepository raceRepo;
    private readonly ISubraceRepository subraceRepo;
    private readonly IAbilityRepository abilityRepo;
    private readonly HttpClient client = new();

    public ExternalSpeciesService(IRaceRepository raceRepo, ISubraceRepository subraceRepo, IAbilityRepository abilityRepo)
    {
        this.raceRepo = raceRepo;
        this.subraceRepo = subraceRepo;
        this.abilityRepo = abilityRepo;
    }

    public async Task FetchExternalRacesAsync(CancellationToken cancellationToken = default)
    {
        if ((await raceRepo.GetAllAsync()).Count > 0)
        {
            Console.WriteLine("Races already exist in the database. Skipping fetch.");
            return;
        }
        
        var getListResponse = await client.GetAsync("https://www.dnd5eapi.co/api/2014/races/", cancellationToken);
        var result = await JsonSerializer.DeserializeAsync<EIndexListDto>(getListResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

        if (result is null || result.Results.Count == 0)
        {
            Console.WriteLine("No races found in external API.");
            return;
        }

        foreach (var item in result.Results)
        {
            var getResponse = await client.GetAsync($"https://www.dnd5eapi.co/api/2014/races/{item.Index}", cancellationToken);
            var eRace = await JsonSerializer.DeserializeAsync<ERaceDto>(getResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

            if (eRace is null)
            {
                Console.WriteLine($"Failed to deserialize race {item.Index}.");
                continue;
            }
            if (await raceRepo.GetByNameAsync(eRace.Name) is not null)
            {
                Console.WriteLine($"Race {eRace.Name} already exists. Skipping.");
                continue;
            }

            var description = new RaceDescription
            {
                GeneralDescription = "",
                AgingDescription = eRace.Age,
                CommonAlignmentDescription = eRace.PreferedAlignment,
                SizeDescription = eRace.SizeDescription,
                LanguageDescription = eRace.AvailableLanguagesDescription
            };

            var race = new Race
            {
                Name = eRace.Name,
                Speed = eRace.Speed,
                RaceDescription = description,
                Size = ValidationUtil.ParseEnumOrThrow<CreatureSize>(eRace.Size),
                Traits = [],
                SubRaces = []
            };

            await raceRepo.CreateAsync(race);

            await AddAbilityScoreBonusesAsTraitAsync(eRace, race, raceRepo);
            await FetchExternalSubracesAsync(race, eRace.Subraces, cancellationToken);
        }
    }

    // TODO: For subraces, switch to an API with better data coverage
    public async Task FetchExternalSubracesAsync(Race race, List<EIndexDto> subraceIndexList, CancellationToken cancellationToken = default)
    {
        if (subraceIndexList.Count == 0)
            return;

        foreach (var item in subraceIndexList)
        {
            var getResponse = await client.GetAsync($"https://www.dnd5eapi.co/api/2014/subraces/{item.Index}", cancellationToken);
            var eSubrace = await JsonSerializer.DeserializeAsync<ESubraceDto>(getResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

            if (eSubrace is null)
            {
                Console.WriteLine($"Failed to deserialize subrace {item.Index}.");
                continue;
            }
            if (await subraceRepo.GetByNameAsync(eSubrace.Name) is not null)
            {
                Console.WriteLine($"Subrace {eSubrace.Name} already exists. Skipping.");
                continue;
            }

            var description = new RaceDescription
            {
                GeneralDescription = eSubrace.Description
            };

            var subrace = new Subrace
            {
                Name = eSubrace.Name,
                Speed = race.Speed,
                RaceDescription = description,
                Size = race.Size,
                Traits = [],
                ParentRace = race,
                ParentRaceId = race.Id
            };

            await subraceRepo.CreateAsync(subrace);
            await AddAbilityScoreBonusesAsTraitAsync(eSubrace, subrace, subraceRepo);
        }
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
            var ability = await abilityRepo.GetByShortNameAsync(abilityIncrease.AbilityScore.Index)
                ?? throw new ArgumentException($"Ability with short name {abilityIncrease.AbilityScore.Index} not found.");

            abilityIncreases.Add(new AbilityValue
            {
                AbilityId = ability.Id,
                Value = abilityIncrease.Bonus,
                Type = ValidationUtil.ParseEnumOrThrow<AbilityType>(ability.FullName)
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
            IsHomebrew = false,
            FromRace = species,
            RaceId = species.Id,
            AbilityIncreases = abilityIncreases
        };

        species.Traits.Add(increaseTrait);
        await speciesRepo.UpdateAsync(species);
    }
}