using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;

namespace DndWebApp.Api.Repositories;

public class PassiveEffectRepository(AppDbContext context) : EfRepository<PassiveEffect>(context)
{
}

public class TraitRepository(AppDbContext context) : EfRepository<Trait>(context)
{
}

public class FeatRepository(AppDbContext context) : EfRepository<Feat>(context)
{
}

public class ClassFeatureRepository(AppDbContext context) : EfRepository<ClassFeature>(context)
{
}