using System.Text.Json.Serialization;

namespace Api.External.Shared;

public class EIndexListDto
{
    [JsonPropertyName("results")]
    public required List<EIndexDto> Results { get; set; }
}