using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Repositories.Features;
using DndWebApp.Api.Services.Generic;
using DndWebApp.Api.Services.Util;

namespace DndWebApp.Api.Services.Features;

public class FeatService : IService<Feat, FeatDto, FeatDto>
{
    protected IFeatRepository repo;

    public FeatService(IFeatRepository repo)
    {
        this.repo = repo;
    }

    public async Task<Feat> CreateAsync(FeatDto dto)
    {
        ValidationUtil.ValidateRequiredString(dto.Name);
        ValidationUtil.ValidateRequiredString(dto.Description);

        var feat = new Feat
        {
            Name = dto.Name,
            Description = dto.Description,
            IsHomebrew = dto.IsHomebrew
        };

        // TODO
        // Method that parses description into data that fills the lists in AFeature

        return await repo.CreateAsync(feat);
    }

    public async Task DeleteAsync(int id)
    {
        var feat = await repo.GetByIdAsync(id) ?? throw new NullReferenceException("Feat could not be found");
        await repo.DeleteAsync(feat);
    }

    public async Task<ICollection<Feat>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<Feat> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) ?? throw new NullReferenceException("Feat could not be found");
    }

    public async Task UpdateAsync(FeatDto dto)
    {
        ValidationUtil.ValidateRequiredString(dto.Name);
        ValidationUtil.ValidateRequiredString(dto.Description);

        var feat = await repo.GetByIdAsync(dto.Id) ?? throw new NullReferenceException("Feat could not be found");

        feat.Name = dto.Name;
        feat.Description = dto.Description;

        // TODO
        // Method that parses description into data that fills the lists in AFeature

        await repo.UpdateAsync(feat);
    }
}