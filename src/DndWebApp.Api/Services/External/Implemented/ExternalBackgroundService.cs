namespace DndWebApp.Api.Services.External.Implemented;

using System.Text.Json;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.DTOs.ExternalDTOs;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.External.Interfaces;
using DndWebApp.Api.Services.Interfaces;
using DndWebApp.Api.Services.Util;

public class ExternalBackgroundService : IExternalBackgroundService
{
    private readonly IBackgroundRepository repo;
    private readonly HttpClient client = new();

    public ExternalBackgroundService(IBackgroundRepository repo)
    {
        this.repo = repo;
    }

    public async Task FetchExternalBackgroundsAsync(CancellationToken cancellationToken = default)
    {
        if ((await repo.GetAllAsync()).Count > 0)
            return; // Abilities already exist in the database. Skipping fetch.

        var getListResponse = await client.GetAsync("https://api.open5e.com/v2/backgrounds/", cancellationToken);
        var backgroundResults = await JsonSerializer.DeserializeAsync<List<EBackgroundDto>>(getListResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

        if (backgroundResults is null || backgroundResults.Count == 0)
            throw new InvalidOperationException("No backgrounds found in external API.");

        foreach (var eBackground in backgroundResults)
        {
            if (eBackground is null)
                throw new InvalidOperationException($"Failed to deserialize background.");

            var background = new Background
            {
                Type = NormalizationUtil.ParseEnumOrThrow<BackgroundType>(eBackground.Name),
                Name = eBackground.Name,
                Description = string.Join("\n", eBackground.Description),
                StartingCurrency = new()
            };

            await repo.CreateAsync(background);
            ParseBackgroundBenefits(background);
        }
    }
    
    // TODO: Parses Features, StartingItems, StartingCurrency and StartingItemOption
    private static void ParseBackgroundBenefits(Background background)
    {
        
    }
}

        