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
    private readonly IAbilityRepository repo;
    private readonly HttpClient client = new();

    public ExternalAbilityService(IAbilityRepository repo)
    {
        this.repo = repo;
    }

    public async Task FetchExternalAbilitiesAsync(CancellationToken cancellationToken = default)
    {
        if ((await repo.GetAllAsync()).Count > 0)
        {
            Console.WriteLine("Abilities already exist in the database. Skipping fetch.");
            return;
        }
        
        var getListResponse = await client.GetAsync("https://www.dnd5eapi.co/api/ability-scores/", cancellationToken);
        var result = await JsonSerializer.DeserializeAsync<EIndexListDto>(getListResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

        if (result is null || result.Results.Count == 0)
        {
            Console.WriteLine("No abilities found in external API.");
            return;
        }

        foreach (var item in result.Results)
        {
            var getResponse = await client.GetAsync($"https://www.dnd5eapi.co/api/ability-scores/{item.Index}", cancellationToken);
            var eAbility = await JsonSerializer.DeserializeAsync<EAbilityDto>(getResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

            if (eAbility is null)
            {
                Console.WriteLine($"Failed to deserialize ability {item.Index}.");
                continue;
            }
            if (await repo.GetByShortNameAsync(eAbility.FullName) is not null)
            {
                Console.WriteLine($"Ability {eAbility.FullName} already exists. Skipping.");
                continue;
            }

            var ability = new Ability
            {
                FullName = eAbility.FullName,
                ShortName = eAbility.Name,
                Description = string.Join("\n", eAbility.Description),
                Skills = []
            };

            await repo.CreateAsync(ability);
        }
    }
}

        