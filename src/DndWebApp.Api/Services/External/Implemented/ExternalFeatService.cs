using System.Text.Json;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.DTOs.ExternalDtos;
using DndWebApp.Api.Models.ExternalDTOs;
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
            Console.WriteLine("Feats already exist in the database. Skipping fetch.");
            return;
        }
        
        var getListResponse = await client.GetAsync("https://www.dnd5eapi.co/api/feats/", cancellationToken);
        var listResult = await JsonSerializer.DeserializeAsync<EFeatListDto>(getListResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);
        var featResults = listResult?.Results ?? [];
        
        if (featResults.Count == 0)
        {
            Console.WriteLine("No feats found in external API.");
            return;
        }

        getListResponse = await client.GetAsync("https://www.dnd5eapi.co/api/feats/?page=2", cancellationToken);
        listResult = await JsonSerializer.DeserializeAsync<EFeatListDto>(getListResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);
        var featResults2 = listResult?.Results ?? [];

        if (featResults.Count == 0)
        {
            Console.WriteLine("No feats found on page 2 in external API.");
            return;
        }

        featResults.AddRange(featResults2);

        foreach (var eFeat in featResults)
        {
            if (eFeat is null)
            {
                Console.WriteLine($"Failed to deserialize feat.");
                continue;
            }
            if (await repo.GetByNameAsync(eFeat.Name) is not null)
            {
                Console.WriteLine($"Feat {eFeat.Name} already exists. Skipping.");
                continue;
            }

            var feat = new Feat
            {
                Name = eFeat.Name,
                Description = string.Join("\n", eFeat.Description),
                Prerequisite = eFeat.Prerequisite ?? "",
            };

            // TODO: Parse feat effects
            // feat.AbilityIncreases, feat.Languages, feat.WeaponProficiencyChoices, ..... = ParseBenefits(eFeat.Benefits);

            await repo.CreateAsync(feat);
        }
    }
}

        