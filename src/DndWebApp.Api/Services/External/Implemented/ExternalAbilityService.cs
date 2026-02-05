namespace DndWebApp.Api.Services.External.Implemented;

using System.Text.Json;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.ExternalDTOs;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.External.Interfaces;
using DndWebApp.Api.Services.Util;

public class ExternalAbilityService(IAbilityRepository repo, ILogger<ExternalAbilityService> logger) : IExternalAbilityService
{
    private readonly HttpClient client = new();

    public async Task FetchExternalAbilitiesAsync(CancellationToken cancellationToken = default)
    {
        var existingCount = (await repo.GetAllAsync()).Count;
        if (existingCount > 0)
        {
            logger.LogInformation("Skipping external abilities fetch. ExistingCount: {ExistingCount}", existingCount);
            return;
        }

        logger.LogInformation("Fetching external abilities.");
        var getListResponse = await client.GetAsync("https://www.dnd5eapi.co/api/ability-scores/", cancellationToken);
        var result = await JsonSerializer.DeserializeAsync<EIndexListDto>(getListResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

        if (result is null || result.Results.Count == 0)
            throw new InvalidOperationException("No abilities found in external API.");

        foreach (var item in result.Results)
        {
            var getResponse = await client.GetAsync($"https://www.dnd5eapi.co/api/ability-scores/{item.Index}", cancellationToken);
            var eAbility = await JsonSerializer.DeserializeAsync<EAbilityDto>(getResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

            if (eAbility is null)
                throw new InvalidOperationException($"Failed to deserialize ability {item.Index}.");

            var ability = new Ability
            {
                FullName = eAbility.FullName,
                ShortName = eAbility.Name,
                Description = string.Join("\n", eAbility.Description),
                Skills = []
            };

            await repo.CreateAsync(ability);
        }

        logger.LogInformation("Successfully fetched external abilities. Count: {AbilityCount}", result.Results.Count);
    }
}

        