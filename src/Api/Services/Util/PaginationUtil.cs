using Api.Models.DTOs.ResponseDtos;

namespace Api.Services.Util;

public static class PaginationUtil
{
    public static string? GetNext(PaginationRequestDto dto, int itemCount, string route)
    {
        var totalPages = (int)Math.Ceiling((decimal)itemCount / dto.PageSize);

        if(dto.Page >= totalPages)
            return null;

        return $"{route}?page={dto.Page + 1}&pageSize={dto.PageSize}";
    }

    public static string? GetPrev(PaginationRequestDto dto, string route)
    {
        if(dto.Page <= 1)
            return null;

        return $"{route}?page={dto.Page - 1}&pageSize={dto.PageSize}";
    }
}