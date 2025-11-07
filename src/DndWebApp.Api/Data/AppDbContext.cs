using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Models.World;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DndWebApp.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Character> Characters { get; set; }
    public DbSet<Inventory> Inventories { get; set; }

    public DbSet<Ability> AbilityScores { get; set; }
    public DbSet<Skill> Skills { get; set; }

    public DbSet<Alignment> Alignments { get; set; }
    public DbSet<Language> Languages { get; set; }

    public DbSet<Class> Classes { get; set; }
    public DbSet<ClassLevel> ClassLevels { get; set; }

    public DbSet<Race> Races { get; set; }
    public DbSet<Subrace> SubRaces { get; set; }
    public DbSet<Background> Backgrounds { get; set; }

    public DbSet<Trait> Traits { get; set; }
    public DbSet<Feat> Feats { get; set; }
    public DbSet<ClassFeature> ClassFeatures { get; set; }
    public DbSet<BackgroundFeature> BackgroundFeatures { get; set; }

    public DbSet<Item> Items { get; set; }
    public DbSet<Weapon> Weapons { get; set; }
    public DbSet<Armor> Armor { get; set; }
    public DbSet<Tool> Tools { get; set; }

    public DbSet<Spell> Spells { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AFeature>().ConfigureProficiencyChoices();
        modelBuilder.Entity<Character>().ConfigureProficiencies();

        modelBuilder.Entity<Class>().OwnsMany(c => c.StartingEquipmentOptions, o => o.ToJson("StartingEquipment"));

        modelBuilder.Entity<Background>().OwnsMany(c => c.StartingItemsOptions, o => o.ToJson("StartingItemOptions"));

        modelBuilder.Entity<ClassLevel>()
            .OwnsMany(c => c.ClassSpecificSlotsAtLevel, slot =>
            {
                slot.WithOwner().HasForeignKey("ClassLevelId");
                slot.HasKey(s => s.Id); // Shadow keys messed with test
            });

        modelBuilder.Entity<Tool>().OwnsMany(t => t.Activities);
        modelBuilder.Entity<Tool>().OwnsMany(t => t.Properties);
    }
}

public static class FeatureConfigurationExtensions
{
    public static void ConfigureProficiencyChoices(this EntityTypeBuilder<AFeature> builder)
    {
        builder.OwnsMany(f => f.SkillProficiencyChoices, c => c.ToJson("SkillOptions"));
        builder.OwnsMany(f => f.WeaponCategoryProficiencyChoices, c => c.ToJson("WeaponCategoryOptions"));
        builder.OwnsMany(f => f.WeaponTypeProficiencyChoices, c => c.ToJson("WeaponTypeOptions"));
        builder.OwnsMany(f => f.ToolProficiencyChoices, c => c.ToJson("ToolOptions"));
        builder.OwnsMany(f => f.LanguageChoices, c => c.ToJson("LanguageOptions"));
        builder.OwnsMany(f => f.ArmorProficiencyChoices, c => c.ToJson("ArmorOptions"));
        builder.OwnsMany(f => f.AbilityIncreaseChoices, c => c.ToJson("AbilityOptions"));
    }

    public static void ConfigureProficiencies(this EntityTypeBuilder<Character> builder)
    {
        builder.OwnsMany(c => c.AbilityScores, s => s.ToJson("AbilityScores"));
        builder.OwnsMany(c => c.ArmorProficiencies, p => p.ToJson("ArmorProficiencies"));
        builder.OwnsMany(c => c.WeaponCategoryProficiencies, p => p.ToJson("WeaponCategoryProficiencies"));
        builder.OwnsMany(c => c.WeaponTypeProficiencies, p => p.ToJson("WeaponTypeProficiencies"));
        builder.OwnsMany(c => c.DamageAffinities, p => p.ToJson("DamageAffinities"));
        builder.OwnsMany(c => c.Languages, p => p.ToJson("Languages"));
        builder.OwnsMany(c => c.SavingThrows, p => p.ToJson("SavingThrows"));
        builder.OwnsMany(c => c.SkillProficiencies, p => p.ToJson("SkillProficiencies"));
        builder.OwnsMany(c => c.ToolProficiencies, p => p.ToJson("ToolProficiencies"));
    }
}