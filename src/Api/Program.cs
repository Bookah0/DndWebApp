

using Api.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Api.Domain.Users.Models;
using Api.Domain.Users.Services;
using Api.Domain.Shared.Repositories;
using Api.Domain.Characters.Repositories;
using Api.Domain.Backgrounds.Repositories;
using Api.Domain.Abilities.Repositories;
using Api.Domain.Alignments.Repositories;
using Api.Domain.Languages.Repositories;
using Api.Domain.Skills.Repositories;
using Api.Domain.Spells.Repositories;
using Api.Domain.Items.Repositories;
using Api.Domain.Classes.Repositories;
using Api.Domain.Classes.Models;
using Api.Domain.Feats.Models;
using Api.Domain.Feats.Repositories;
using Api.Domain.Species.Repositories;
using Api.Domain.Backgrounds.Models;
using Api.Domain.Species.Models;
using Api.Domain.Abilities.Services;
using Api.Domain.Shared.Services;
using Api.Domain.Items.Services;
using Api.External.Languages;
using Api.Infrastructure.Middleware.ExceptionHandling;
using Api.External.Items;
using Api.External.Abilities;
using Api.External.Alignments;
using Api.External.Backgrounds;
using Api.External.Classes;
using Api.External.Feats;
using Api.External.Skills;
using Api.External.Species;
using Api.External.Spells;
using Api.Domain.Classes.Services;
using Api.Domain.Alignments.Services;
using Api.Domain.Backgrounds.Services;
using Api.Domain.Skills.Services;
using Api.Domain.Spells.Services;
using Api.Domain.Species.Services;
using Api.Domain.Characters.Services;
using Api.Domain.Languages.Services;
using Api.Domain.Shared.DTOs;
using Api.Domain.Feats.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
    npgsql => npgsql.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddIdentity<User, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

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

builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IToolRepository, ToolRepository>();
builder.Services.AddScoped<IArmorRepository, ArmorRepository>();
builder.Services.AddScoped<IWeaponRepository, WeaponRepository>();

builder.Services.AddScoped<IRaceRepository, RaceRepository>();
builder.Services.AddScoped<ISubraceRepository, SubraceRepository>();

builder.Services.AddScoped<IClassLevelRepository, ClassLevelRepository>();
builder.Services.AddScoped<IBaseClassRepository, ClassRepository>();
builder.Services.AddScoped<ISubclassRepository, SubclassRepository>();

// Core services
builder.Services.AddScoped<IAbilityService, AbilityService>();
builder.Services.AddScoped<IAlignmentService, AlignmentService>();
builder.Services.AddScoped<IBackgroundService, Api.Domain.Backgrounds.Services.BackgroundService>();
builder.Services.AddScoped<ICharacterService, CharacterService>();
builder.Services.AddScoped<IClassLevelService, ClassLevelService>();
builder.Services.AddScoped<IBaseClassService, BaseClassService>();
builder.Services.AddScoped<ILanguageService, LanguageService>();
builder.Services.AddScoped<IRaceService, RaceService>();
builder.Services.AddScoped<ISkillService, SkillService>();
builder.Services.AddScoped<ISpellService, SpellService>();
builder.Services.AddScoped<ISubclassService, SubclassService>();
builder.Services.AddScoped<ISubraceService, SubraceService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// Feature services
//builder.Services.AddScoped(typeof(IBaseFeatureService<>), typeof(BaseFeatureService<>));
builder.Services.AddScoped<IFeatureService<BackgroundFeature, CreateBackgroundFeatureRequestDto, UpdateBackgroundFeatureRequestDto>, BackgroundFeatureService>();
builder.Services.AddScoped<IFeatureService<ClassFeature, CreateClassFeatureRequestDto, UpdateClassFeatureRequestDto>, ClassFeatureService>();
builder.Services.AddScoped<IFeatureService<Feat, CreateFeatRequestDto, UpdateFeatRequestDto>, FeatService>();
builder.Services.AddScoped<IFeatureService<Trait, CreateTraitRequestDto, UpdateTraitRequestDto>, TraitService>();

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

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandler>();
app.UseHttpsRedirection();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();

    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
    await userService.InitRolesAsync();
    var fetchExternalData = true;

    if (app.Environment.IsDevelopment())
    {
        if(fetchExternalData)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Starting external data fetch");
            
            var itemService = scope.ServiceProvider.GetRequiredService<IExternalItemService>();
            await itemService.FetchExternalBasicItemsAsync();
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
            var languageService = scope.ServiceProvider.GetRequiredService<IExternalLanguageService>();
            await languageService.FetchExternalLanguagesAsync();
            var skillService = scope.ServiceProvider.GetRequiredService<IExternalSkillService>();
            await skillService.FetchExternalSkillsAsync();
        }
    }
}

app.Use(async (context, next) =>
{
    Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
    try
    {
        await next();
        Console.WriteLine($"Response: {context.Response.StatusCode}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception: {ex}");
        throw;
    }
});

app.Run();