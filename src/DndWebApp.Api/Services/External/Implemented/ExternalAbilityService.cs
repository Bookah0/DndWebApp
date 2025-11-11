using System.Text.Json;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.DTOs.ExternalDtos;
using DndWebApp.Api.Models.ExternalDTOs;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.External.Interfaces;
using DndWebApp.Api.Services.Interfaces;

public class ExternalAbilityService : IExternalAbilityService
{
    private readonly IAbilityRepository abilityRepository;
    private readonly HttpClient client = new();

    public ExternalAbilityService(IAbilityRepository abilityRepository)
    {
        this.abilityRepository = abilityRepository;
    }

    public async Task FetchExternalAbilitiesAsync(CancellationToken cancellationToken = default)
    {
        var getListResponse = await client.GetAsync("https://www.dnd5eapi.co/api/ability-scores/", cancellationToken);
        var result = await JsonSerializer.DeserializeAsync<EListDto>(getListResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

        if (result is null || result.results.Count == 0)
        {
            Console.WriteLine("No abilities found in external API.");
            return;
        }

        foreach (var item in result.results)
        {
            var getResponse = await client.GetAsync($"https://www.dnd5eapi.co/api/ability-scores/{item.index}", cancellationToken);
            var eAbility = await JsonSerializer.DeserializeAsync<EAbilityDto>(getResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

            if (eAbility is not null && await abilityRepository.GetByNameAsync(eAbility.full_name) is not null)
            {
                Console.WriteLine($"Ability {eAbility.full_name} already exists. Skipping.");
                continue;
            }

            var ability = new Ability
            {
                FullName = eAbility!.full_name,
                ShortName = eAbility.name,
                Description = string.Join("\n", eAbility.desc),
                Skills = []
            };

            await abilityRepository.CreateAsync(ability);
        }
    }
}

        