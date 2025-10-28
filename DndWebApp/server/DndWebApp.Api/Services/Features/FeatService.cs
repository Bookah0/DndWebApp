using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Repositories.Features;
using DndWebApp.Api.Services.Generic;

namespace DndWebApp.Api.Services.Features;

public class FeatService : IService<Feat, FeatDto, FeatDto>
{
    protected IFeatRepository repo;

    public FeatService(IFeatRepository repo)
    {
        this.repo = repo;
    }

    public Task<Feat> CreateAsync(FeatDto entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<Feat>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Feat> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(FeatDto updatedEntity)
    {
        throw new NotImplementedException();
    }
}