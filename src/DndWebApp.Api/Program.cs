using DndWebApp.Api.Data;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Repositories.Implemented;
using DndWebApp.Api.Repositories.Implemented.Classes;
using DndWebApp.Api.Repositories.Implemented.Species;
using DndWebApp.Api.Repositories.Implemented.Features;
using DndWebApp.Api.Repositories.Implemented.Items;
using DndWebApp.Api.Repositories.Implemented.Spells;
using DndWebApp.Api.Services.Interfaces;
using DndWebApp.Api.Services.Implemented;
using DndWebApp.Api.Services.Implemented.Features;
using DndWebApp.Api.Services.Interfaces.Features;
using DndWebApp.Api.Services.External.Interfaces;
using DndWebApp.Api.Services.External.Implemented;
using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Services.Interfaces.Species;
using DndWebApp.Api.Services.Implemented.Classes;
using DndWebApp.Api.Services.Interfaces.Items;
using DndWebApp.Api.Services.Implemented.Items;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.Characters;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
    npgsql => npgsql.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));
builder.Services.AddAutoMapper(typeof(Program));

// Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();
builder.Services.AddScoped<IBackgroundRepository, BackgroundRepository>();
builder.Services.AddScoped<IAbilityRepository, AbilityRepository>();
builder.Services.AddScoped<IAlignmentRepository, AlignmentRepository>();
builder.Services.AddScoped<ILanguageRepository, LanguageRepository>();
builder.Services.AddScoped<ISkillRepository, SkillRepository>();
builder.Services.AddScoped<ISpellRepository, SpellRepository>();

builder.Services.AddScoped<IFeatureRepository<ClassFeature>, ClassFeatureRepository>();
builder.Services.AddScoped<IFeatureRepository<Feat>, FeatRepository>();
builder.Services.AddScoped<IFeatureRepository<BackgroundFeature>, BackgroundFeatureRepository>();
builder.Services.AddScoped<IFeatureRepository<Trait>, TraitRepository>();

builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IToolRepository, ToolRepository>();

builder.Services.AddScoped<IRaceRepository, RaceRepository>();
builder.Services.AddScoped<ISubraceRepository, SubraceRepository>();

builder.Services.AddScoped<IClassLevelRepository, ClassLevelRepository>();
builder.Services.AddScoped<IClassRepository, ClassRepository>();
builder.Services.AddScoped<ISubclassRepository, SubclassRepository>();

// Core services
builder.Services.AddScoped<IAbilityService, AbilityService>();
builder.Services.AddScoped<IAlignmentService, AlignmentService>();
builder.Services.AddScoped<IBackgroundService, DndWebApp.Api.Services.Implemented.BackgroundService>();
builder.Services.AddScoped<ICharacterService, CharacterService>();
builder.Services.AddScoped<IClassLevelService, ClassLevelService>();
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<ILanguageService, LanguageService>();
builder.Services.AddScoped<IRaceService, RaceService>();
builder.Services.AddScoped<ISkillService, SkillService>();
builder.Services.AddScoped<ISpellService, SpellService>();
builder.Services.AddScoped<ISubclassService, SubclassService>();
builder.Services.AddScoped<ISubraceService, SubraceService>();

// Feature services
//builder.Services.AddScoped(typeof(IBaseFeatureService<>), typeof(BaseFeatureService<>));
builder.Services.AddScoped<IFeatureService<BackgroundFeature, BackgroundFeatureDto>, BackgroundFeatureService>();
builder.Services.AddScoped<IFeatureService<ClassFeature, ClassFeatureDto>, ClassFeatureService>();
builder.Services.AddScoped<IFeatureService<Feat, FeatDto>, FeatService>();
builder.Services.AddScoped<IFeatureService<Trait, TraitDto>, TraitService>();

// Item services
builder.Services.AddScoped<IArmorService, ArmorService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IToolService, ToolService>();
builder.Services.AddScoped<IWeaponService, WeaponService>();

// External services
builder.Services.AddScoped<IExternalAbilityService, ExternalAbilityService>();
builder.Services.AddScoped<IExternalAlignmentService, ExternalAlignmentService>();
builder.Services.AddScoped<IExternalBackgroundService, ExternalBackgroundService>();
builder.Services.AddScoped<IExternalClassService, ExternalClassService>();
builder.Services.AddScoped<IExternalFeatService, ExternalFeatService>();
builder.Services.AddScoped<IExternalItemService, ExternalItemService>();
builder.Services.AddScoped<IExternalLanguageService, ExternalLanguageService>();
builder.Services.AddScoped<IExternalSkillService, ExternalSkillService>();
builder.Services.AddScoped<IExternalSpeciesService, ExternalSpeciesService>();
builder.Services.AddScoped<IExternalSpellService, ExternalSpellService>();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
app.UseMiddleware<ExceptionHandler>();
app.UseHttpsRedirection();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
    
    if (app.Environment.IsDevelopment())
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Starting external data fetch");
        
        var alignmentService = scope.ServiceProvider.GetRequiredService<IExternalAlignmentService>();
        await alignmentService.FetchExternalAlignmentsAsync();
        var abilityService = scope.ServiceProvider.GetRequiredService<IExternalAbilityService>();
        await abilityService.FetchExternalAbilitiesAsync();
        var backgroundService = scope.ServiceProvider.GetRequiredService<IExternalBackgroundService>();
        await backgroundService.FetchExternalBackgroundsAsync();
        var featService = scope.ServiceProvider.GetRequiredService<IExternalFeatService>();
        await featService.FetchExternalFeatsAsync();
        var spellService = scope.ServiceProvider.GetRequiredService<IExternalSpellService>();
        await spellService.FetchExternalSpellsAsync();
        var speciesService = scope.ServiceProvider.GetRequiredService<IExternalSpeciesService>();
        await speciesService.FetchExternalRacesAsync();
        var classService = scope.ServiceProvider.GetRequiredService<IExternalClassService>();
        await classService.FetchExternalClassesAsync();
        var itemService = scope.ServiceProvider.GetRequiredService<IExternalItemService>();
        await itemService.FetchExternalBasicItemsAsync();
        var languageService = scope.ServiceProvider.GetRequiredService<IExternalLanguageService>();
        await languageService.FetchExternalLanguagesAsync();
        var skillService = scope.ServiceProvider.GetRequiredService<IExternalSkillService>();
        await skillService.FetchExternalSkillsAsync();
    }
}

app.Run();