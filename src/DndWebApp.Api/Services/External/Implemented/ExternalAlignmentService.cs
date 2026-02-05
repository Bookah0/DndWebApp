namespace DndWebApp.Api.Services.External.Implemented;

using System.Text.Json;
using DndWebApp.Api.Models.DTOs.ExternalDTOs;
using DndWebApp.Api.Models.World;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.External.Interfaces;
using DndWebApp.Api.Services.Util;

public class ExternalAlignmentService(IAlignmentRepository repo, ILogger<ExternalAlignmentService> logger) : IExternalAlignmentService
{
    private readonly HttpClient client = new();

    public async Task FetchExternalAlignmentsAsync(CancellationToken cancellationToken = default)
    {
        var existingCount = (await repo.GetAllAsync()).Count;
        if (existingCount > 0)
        {
            logger.LogInformation("Skipping external alignments fetch. ExistingCount: {ExistingCount}", existingCount);
            return;
        }
        
        logger.LogInformation("Fetching external alignments.");

        var getListResponse = await client.GetAsync("https://www.dnd5eapi.co/api/2014/alignments/", cancellationToken);
        var result = await JsonSerializer.DeserializeAsync<EIndexListDto>(getListResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

        if (result is null || result.Results.Count == 0)
            throw new InvalidOperationException("No alignment found in external API.");

        foreach (var item in result.Results)
        {
            var getResponse = await client.GetAsync($"https://www.dnd5eapi.co/api/2014/alignments/{item.Index}", cancellationToken);
            var eAlignment = await JsonSerializer.DeserializeAsync<EAlignmentDto>(getResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

            if (eAlignment is null)
                throw new InvalidOperationException($"Failed to deserialize alignment {item.Index}.");

            var alignment = new Alignment
            {
                Name = eAlignment.Name,
                Abbreviation = eAlignment.Abbreviation,
                Description = eAlignment.Description
            };

            await repo.CreateAsync(alignment);
        }

        logger.LogInformation("Successfully fetched external alignments. Count: {AlignmentCount}", result.Results.Count);
    }
}
