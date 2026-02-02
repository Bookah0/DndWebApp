namespace DndWebApp.Api.Services.External.Implemented;

using System.Text.Json;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.ExternalDTOs;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.External.Interfaces;

public class ExternalBackgroundService(IBackgroundRepository repo, ILogger<ExternalBackgroundService> logger) : IExternalBackgroundService
{
    private readonly HttpClient client = new();

    public async Task FetchExternalBackgroundsAsync(CancellationToken cancellationToken = default)
    {
        var existingCount = (await repo.GetAllAsync()).Count;
        if (existingCount > 0)
        {
            logger.LogInformation("Skipping external backgrounds fetch. ExistingCount: {ExistingCount}", existingCount);
            return;
        }

        logger.LogInformation("Fetching external backgrounds.");

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
                Name = eBackground.Name,
                Description = string.Join("\n", eBackground.Description),
                StartingCurrency = new()
            };

            await repo.CreateAsync(background);
            ParseBackgroundBenefits(background);
        }

        logger.LogInformation("Successfully fetched external backgrounds. Count: {BackgroundCount}", backgroundResults.Count);
    }
    
    // TODO: Parses Features, StartingItems, StartingCurrency and StartingItemOption
    private static void ParseBackgroundBenefits(Background background)
    {
        
    }
}

        