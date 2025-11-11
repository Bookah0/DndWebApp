using System.Globalization;
using System.Text.Json;
using DndWebApp.Api.Models.DTOs.ExternalDtos;
using DndWebApp.Api.Models.ExternalDTOs;
using DndWebApp.Api.Models.World;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.External.Interfaces;

namespace DndWebApp.Api.Services.External.Implemented;

public class ExternalAlignmentService : IExternalAlignmentService
{
    private readonly IAlignmentRepository alignmentRepository;
    private readonly HttpClient client = new();
    
    public ExternalAlignmentService(IAlignmentRepository alignmentRepository)
    {
        this.alignmentRepository = alignmentRepository;
    }

    public async Task FetchExternalAlignmentsAsync(CancellationToken cancellationToken = default)
    {
        var getListResponse = await client.GetAsync("https://www.dnd5eapi.co/api/2014/alignments/", cancellationToken);
        var result = await JsonSerializer.DeserializeAsync<EListDto>(getListResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

        if (result is null || result.results.Count == 0)
        {
            Console.WriteLine("No alignment found in external API.");
            return;
        }

        foreach (var item in result.results)
        {
            var getResponse = await client.GetAsync($"https://www.dnd5eapi.co/api/2014/alignments/{item.index}", cancellationToken);
            var eAlignment = await JsonSerializer.DeserializeAsync<EAlignmentDto>(getResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

            if (eAlignment is not null && await alignmentRepository.GetByNameAsync(eAlignment.name) is not null)
            {
                Console.WriteLine($"Alignment {eAlignment.name} already exists. Skipping.");
                continue;
            }

            var alignment = new Alignment
            {
                Name = eAlignment!.name,
                Abbreviation = eAlignment.abbreviation,
                Description = eAlignment.desc
            };

            await alignmentRepository.CreateAsync(alignment);
        }
    }
}
