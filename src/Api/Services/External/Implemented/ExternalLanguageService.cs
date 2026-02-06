namespace Api.Services.External.Implemented;

using System.Text.Json;
using Api.Models.Characters;
using Api.Models.DTOs.ExternalDTOs;
using Api.Repositories.Interfaces;
using Api.Services.External.Interfaces;

public class ExternalLanguageService(ILanguageRepository repo, ILogger<ExternalLanguageService> logger) : IExternalLanguageService
{
    private readonly HttpClient client = new();

    public async Task FetchExternalLanguagesAsync(CancellationToken cancellationToken = default)
    {
        if ((await repo.GetAllAsync()).Count > 0)
        {
            logger.LogInformation("Languages already exist in the database. Skipping fetch.");
            return;
        }
        
        logger.LogInformation("Fetching external languages.");

        var getListResponse = await client.GetAsync("https://www.dnd5eapi.co/api/2014/languages/", cancellationToken);
        var result = await JsonSerializer.DeserializeAsync<EIndexListDto>(getListResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

        if (result is null || result.Results.Count == 0)
        {
            throw new InvalidOperationException("No languages found in external API.");
        }

        foreach (var item in result.Results)
        {
            var getResponse = await client.GetAsync($"https://www.dnd5eapi.co/api/2014/languages/{item.Index}", cancellationToken);
            var eLanguage = await JsonSerializer.DeserializeAsync<ELanguageDto>(getResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

            if (eLanguage is null)
            {
                throw new InvalidOperationException($"Failed to deserialize language {item.Index}.");
            }

            var language = new Language
            {
                Name = eLanguage.Name,
                Family = eLanguage.Type,
                Script = eLanguage.Script,
                TypicalSpeakers = eLanguage.TypicalSpeakers,
                IsExotic = eLanguage.Type == "Exotic"
            };

            await repo.CreateAsync(language);
        }

        logger.LogInformation("Successfully fetched external languages. Count: {LanguageCount}", result.Results.Count);
    }
}

        