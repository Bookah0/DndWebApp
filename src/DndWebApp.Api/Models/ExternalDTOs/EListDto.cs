using DndWebApp.Api.Models.ExternalDTOs;

namespace DndWebApp.Api.Models.DTOs.ExternalDtos;

public class EListDto
{
    public required int count { get; set; }
    public required List<EIndexDto> results { get; set; }
}