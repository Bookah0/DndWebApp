

using Api.Data;
using Api.Middlewares.ExceptionHandling;
using Api.Models.DTOs.Features;
using Api.Models.Features;
using Api.Models.Users;
using Api.Repositories.Implemented;
using Api.Repositories.Implemented.Classes;
using Api.Repositories.Implemented.Features;
using Api.Repositories.Implemented.Items;
using Api.Repositories.Implemented.Species;
using Api.Repositories.Implemented.Spells;
using Api.Repositories.Interfaces;
using Api.Services.External.Implemented;
using Api.Services.External.Interfaces;
using Api.Services.Implemented;
using Api.Services.Implemented.Classes;
using Api.Services.Implemented.Features;
using Api.Services.Implemented.Items;
using Api.Services.Interfaces;
using Api.Services.Interfaces.Features;
using Api.Services.Interfaces.Items;
using Api.Services.Interfaces.Species;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
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
builder.Services.AddScoped<IBackgroundService, Api.Services.Implemented.BackgroundService>();
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