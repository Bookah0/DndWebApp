using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Shared.DTOs;

public class PaginationRequestDto
{
    [Range(1, int.MaxValue)]
    public int PageSize { get; set; } = 40;

    [Range(1, int.MaxValue)]
    public int Page { get; set; } = 1;
}

public class PaginationResponseDto<T> where T : class
{
    public required int ItemCount {get; set;}
    public required int Page {get; set;}
    public required int PageSize { get; set; }
    public required string? Next {get; set;}
    public required string? Prev {get; set;}
    public required ICollection<T> Items {get; set;} = [];
}