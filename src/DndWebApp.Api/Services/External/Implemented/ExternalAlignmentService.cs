using System.Globalization;
using System.Text.Json;
using DndWebApp.Api.Models.DTOs.ExternalDtos;
using DndWebApp.Api.Models.World;
using DndWebApp.Api.Models.World.Enums;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.External.Interfaces;
using DndWebApp.Api.Services.Util;

namespace DndWebApp.Api.Services.External.Implemented;

public class ExternalAlignmentService : IExternalAlignmentService
{
    private readonly IAlignmentRepository repo;
    private readonly HttpClient client = new();
    
    public ExternalAlignmentService(IAlignmentRepository repo)
    {
        this.repo = repo;
    }

    public async Task FetchExternalAlignmentsAsync(CancellationToken cancellationToken = default)
    {
        if ((await repo.GetAllAsync()).Count > 0)
        {
            Console.WriteLine("Alignments already exist in the database. Skipping fetch.");
            return;
        }
        
        var getListResponse = await client.GetAsync("https://www.dnd5eapi.co/api/2014/alignments/", cancellationToken);
        var result = await JsonSerializer.DeserializeAsync<EIndexListDto>(getListResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

        if (result is null || result.Results.Count == 0)
        {
            Console.WriteLine("No alignment found in external API.");
            return;
        }

        foreach (var item in result.Results)
        {
            var getResponse = await client.GetAsync($"https://www.dnd5eapi.co/api/2014/alignments/{item.Index}", cancellationToken);
            var eAlignment = await JsonSerializer.DeserializeAsync<EAlignmentDto>(getResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

            if (eAlignment is null)
            {
                Console.WriteLine($"Failed to deserialize alignment {item.Index}.");
                continue;
            }

            var alignment = new Alignment
            {
                Type = NormalizationUtil.ParseEnumOrThrow<AlignmentType>(eAlignment.Name),
                Name = eAlignment.Name,
                Abbreviation = eAlignment.Abbreviation,
                Description = eAlignment.Description
            };

            await repo.CreateAsync(alignment);
        }
    }
}
