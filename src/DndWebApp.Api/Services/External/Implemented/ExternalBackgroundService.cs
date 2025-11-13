using System.Text.Json;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.DTOs.ExternalDtos;
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
        {
            Console.WriteLine("Backgrounds already exist in the database. Skipping fetch.");
            return;
        }

        var getListResponse = await client.GetAsync("https://api.open5e.com/v2/backgrounds/", cancellationToken);
        var backgroundResults = await JsonSerializer.DeserializeAsync<List<EBackgroundDto>>(getListResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

        if (backgroundResults is null || backgroundResults.Count == 0)
        {
            Console.WriteLine("No backgrounds found in external API.");
            return;
        }

        foreach (var eBackground in backgroundResults)
        {
            if (eBackground is null)
            {
                Console.WriteLine($"Failed to deserialize background.");
                continue;
            }

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

        