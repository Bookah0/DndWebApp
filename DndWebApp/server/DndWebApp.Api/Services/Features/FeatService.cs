using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Repositories.Features;
using DndWebApp.Api.Repositories.Spells;
using DndWebApp.Api.Services.Generic;
using DndWebApp.Api.Services.Util;

namespace DndWebApp.Api.Services.Features;

public class FeatService : BaseFeatureService<Feat>, IService<Feat, FeatDto>
{

    public FeatService(IFeatRepository repo, ISpellRepository spellRepo, ILogger<BaseFeatureService<Feat>> logger) : base(repo, spellRepo, logger)
    {
    }

    public async Task<Feat> CreateAsync(FeatDto dto)
    {
        ValidationUtil.HasContentOrThrow(dto.Name);
        ValidationUtil.HasContentOrThrow(dto.Description);

        var feat = new Feat
        {
            Name = dto.Name,
            Description = dto.Description,
            IsHomebrew = dto.IsHomebrew
        };

        return await repo.CreateAsync(feat);
    }

    public async Task DeleteAsync(int id)
    {
        var feat = await repo.GetByIdAsync(id) ?? throw new NullReferenceException($"Feat with id {id} could not be found");
        await repo.DeleteAsync(feat);
    }

    public async Task<ICollection<Feat>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<Feat> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) ?? throw new NullReferenceException($"Feat with id {id} could not be found");
    }

    public async Task UpdateAsync(FeatDto dto)
    {
        ValidationUtil.HasContentOrThrow(dto.Name);
        ValidationUtil.HasContentOrThrow(dto.Description);

        var feat = await repo.GetByIdAsync(dto.Id) ?? throw new NullReferenceException($"Feat with id {dto.Id} could not be found");

        feat.Name = dto.Name;
        feat.Description = dto.Description;
        await repo.UpdateAsync(feat);
    }

    public Task UpdateCollectionsAsync(FeatDto dto)
    {
        throw new NotImplementedException();
    }

    public ICollection<Feat> SortBy(ICollection<Feat> feats, bool descending = false)
    {
        return SortUtil.OrderByMany(feats, [(f => f.Name)], descending);
    }
}