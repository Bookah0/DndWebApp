using DndWebApp.Api.Repositories;
using DndWebApp.Api.Repositories.Interfaces;

namespace DndWebApp.Api.Services.Util;

public static class ValidationUtil
{
    public static void HasContentOrThrow(string? str)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            throw new ArgumentException($"{nameof(str)} cannot be null, empty, or whitespace.");
        }
    }

    public static void AboveZeroOrThrow(int? num)
    {
        if (num == null || num < 0)
        {
            throw new ArgumentException($"{nameof(num)} cannot be null, zero or negative.");
        }
    }

    public static async Task IdExist<T, C>(int id, T repo) where T : IRepository<C>
    {
        if (await repo.GetByIdAsync(id) == null)
            throw new ArgumentOutOfRangeException(nameof(id), $"Entity of type {typeof(T).Name} with id {id} does not exist.");
    }

    public static async Task IdsExist<T, C>(ICollection<int>? ids, T repo) where T : IRepository<C>
    {
        if (ids is null)
            return;

        foreach (var id in ids)
        {
            if (await repo.GetByIdAsync(id) is null)
                throw new ArgumentOutOfRangeException(nameof(ids), $"Entity of type {typeof(T).Name} with id {id} does not exist.");
        }
    }
}