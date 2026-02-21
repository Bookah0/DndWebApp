using System.Text.Json;
using Api.Domain.Backgrounds.Models;
using Api.Domain.Backgrounds.Repositories;
using Api.External.Shared;

namespace Api.External.Backgrounds;

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
        logger.LogInformation("Received response from external API for backgrounds. StatusCode: {StatusCode}", getListResponse.StatusCode);

        var responseAsString = await getListResponse.Content.ReadAsStringAsync(cancellationToken);
        logger.LogInformation("Response string started with: {ResponseStart}", responseAsString.Substring(0, Math.Min(responseAsString.Length, 100)));

        var backgroundResults = await JsonSerializer.DeserializeAsync<EOpen5eResponseDto<EBackgroundDto>>( getListResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);
        logger.LogInformation("Deserialized background results. Count: {BackgroundCount}", backgroundResults?.Count ?? 0);
        
        if (backgroundResults is null || backgroundResults.Count == 0)
            throw new InvalidOperationException("No backgrounds found in external API.");

        foreach (var eBackground in backgroundResults.Results)
        {
            if (eBackground is null)
                throw new InvalidOperationException($"Failed to deserialize background.");

            logger.LogInformation("Processing background: {BackgroundName}", eBackground.Name);

            var background = new Background
            {
                Name = eBackground.Name,
                Description = string.Join("\n", eBackground.Description),
                StartingCurrency = new(),
           
                CreatedAt = DateTime.UtcNow,
                CreatedBy = null,
                IsHomebrew = false,
                IsPublic = true,
                CloningAllowed = true
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

        