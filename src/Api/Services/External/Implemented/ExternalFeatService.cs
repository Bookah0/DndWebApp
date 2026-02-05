namespace Api.Services.External.Implemented;


using System.Text.Json;
using Api.Models.DTOs.ExternalDTOs;
using Api.Models.Features;
using Api.Repositories.Interfaces;
using Api.Services.External.Interfaces;

public class ExternalFeatService(IFeatureRepository<Feat> repo, ILogger<ExternalFeatService> logger) : IExternalFeatService
{
    private readonly HttpClient client = new();

    public async Task FetchExternalFeatsAsync(CancellationToken cancellationToken = default)
    {
        if ((await repo.GetAllAsync()).Count > 0)
        {
            logger.LogInformation("Feats already exist in the database. Skipping fetch.");
            return;
        }

        logger.LogInformation("Fetching external feats.");
        
        var getListResponse = await client.GetAsync("https://api.open5e.com/v1/feats/", cancellationToken);
        var featResults = await JsonSerializer.DeserializeAsync<EOpen5eResponseDto<EFeatDto>>(getListResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);
        
        if (featResults is null || featResults.Count == 0)
        {
            throw new InvalidOperationException("No feats found in external APIs.");
        }

        foreach (var eFeat in featResults.Results)
        {
            if (eFeat is null)
            {
                throw new InvalidOperationException($"Failed to deserialize feat.");
            }

            var feat = new Feat
            {
                Name = eFeat.Name,
                Description = string.Join("\n", eFeat.Description),
                Prerequisite = eFeat.Prerequisite ?? "",
            };
            await repo.CreateAsync(feat);
        }

        logger.LogInformation("Successfully fetched external feats. Count: {FeatCount}", featResults.Count);
    }

    // TODO: Parses Features
    private static void ParseFeatBenefits(Feat feat)
    {
        
    }
}

        