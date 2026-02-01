using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Repositories;
using DndWebApp.Api.Repositories.Interfaces;

namespace DndWebApp.Api.Services.Util;

// TODO Should be removed once validation is handled through Annotations and inline throws
public static class ValidationUtil
{

    // TODO Should be removed once filtering is implemented for Spells
    public static async Task IdsExist<T, C>(ICollection<int>? ids, T repo) where T : IRepository<C>
    {
        if (ids is null)
            return;

        foreach (var id in ids)
        {
            if (await repo.GetByIdAsync(id) is null)
                throw new NotFoundException($"Entity of type {typeof(T).Name} with id {id} does not exist.");
        }
    }
}