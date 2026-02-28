using Api.Domain.Shared.DTOs;

namespace Api.Domain.Shared.Utils;

public static class PaginationUtil
{
	public static PaginationResponseDto<T> BuildPaginationResponse<T>(ICollection<T> items, PaginationRequestDto? pagination, string route) where T : class
	{
		var itemCount = items.Count;
		return new PaginationResponseDto<T>
		{
			Items = items,
			ItemCount = itemCount,
			Page = pagination?.Page ?? 1,
			PageSize = pagination?.PageSize ?? itemCount,
			Next = GetNext(pagination, itemCount, route),
			Prev = GetPrev(pagination, route)
		};
	}

    public static string? GetNext(PaginationRequestDto? dto, int itemCount, string route)
    {
		if(dto is null)
			return null;

        var totalPages = (int)Math.Ceiling((decimal)itemCount / dto.PageSize);

        if(dto.Page >= totalPages)
            return null;

        return $"{route}?page={dto.Page + 1}&pageSize={dto.PageSize}";
    }

    public static string? GetPrev(PaginationRequestDto? dto, string route)
    {
		if(dto is null)
			return null;

        if(dto.Page <= 1)
            return null;

        return $"{route}?page={dto.Page - 1}&pageSize={dto.PageSize}";
    }
}