namespace DndWebApp.Api.Services.External.Implemented;


using System.Text.Json;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.DTOs.ExternalDTOs;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.External.Interfaces;
using DndWebApp.Api.Services.Interfaces;

public class ExternalFeatService : IExternalFeatService
{
    private readonly IFeatRepository repo;
    private readonly HttpClient client = new();

    public ExternalFeatService(IFeatRepository repo)
    {
        this.repo = repo;
    }

    public async Task FetchExternalFeatsAsync(CancellationToken cancellationToken = default)
    {
        if ((await repo.GetAllAsync()).Count > 0)
        {
            throw new InvalidOperationException("Feats already exist in the database. Skipping fetch.");
        }
        
        var getListResponse = await client.GetAsync("https://www.dnd5eapi.co/api/feats/", cancellationToken);
        var featResults = await JsonSerializer.DeserializeAsync<List<EFeatDto>>(getListResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);
        
        if (featResults is null || featResults.Count == 0)
        {
            throw new InvalidOperationException("No feats found on page 1 in external API.");
        }

        getListResponse = await client.GetAsync("https://www.dnd5eapi.co/api/feats/?page=2", cancellationToken);
        var featResults2 = await JsonSerializer.DeserializeAsync<List<EFeatDto>>(getListResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

        if (featResults2 is null || featResults2.Count == 0)
        {
            throw new InvalidOperationException("No feats found on page 2 in external API.");
        }

        featResults.AddRange(featResults2);

        foreach (var eFeat in featResults)
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
    }

    // TODO: Parses Features
    private static void ParseFeatBenefits(Feat feat)
    {
        
    }
}

        