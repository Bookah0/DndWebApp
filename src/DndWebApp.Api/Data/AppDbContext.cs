using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Models.World;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
    public DbSet<Subclass> Subclasses { get; set; }
    public DbSet<ClassLevel> ClassLevels { get; set; }

    public DbSet<Race> Races { get; set; }
    public DbSet<Subrace> Subraces { get; set; }
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

        modelBuilder.Entity<ClassLevel>()
            .OwnsMany(c => c.ClassSpecificSlotsAtLevel, slot =>
            {
                slot.HasKey(s => s.Id);
                slot.WithOwner().HasForeignKey("ClassLevelId");
            });

        modelBuilder.Entity<Class>()
            .OwnsMany(c => c.StartingEquipmentOptions, opt =>
            {
                opt.HasKey(o => o.Id);
                opt.WithOwner().HasForeignKey("ClassId");
            });

        modelBuilder.Entity<Background>()
            .OwnsMany(b => b.StartingItemsOptions, opt =>
            {
                opt.HasKey(o => o.Id);
                opt.WithOwner().HasForeignKey("BackgroundId");
            });

        modelBuilder.Entity<Tool>().OwnsMany(t => t.Activities);
        modelBuilder.Entity<Tool>().OwnsMany(t => t.Properties);

        modelBuilder.Entity<Spell>()
            .HasMany(s => s.Classes)
            .WithMany()
            .UsingEntity(j => j.ToTable("SpellClasses"));

        modelBuilder.Entity<Ability>().ToTable("AbilityScores");
    }
}

public static class FeatureConfigurationExtensions
{
    public static void ConfigureProficiencyChoices(this EntityTypeBuilder<AFeature> builder)
    {
        builder.HasMany(f => f.AbilityIncreases)
            .WithMany()
            .UsingEntity(j => j.ToTable("AbilityIncreases"));

        builder.HasMany(f => f.AbilityIncreaseOptions)
            .WithMany()
            .UsingEntity(j => j.ToTable("AbilityIncreaseOptions"));

        builder.HasMany(c => c.SpellsGained)
            .WithMany()
            .UsingEntity(j => j.ToTable("SpellsGained"));

        builder.OwnsMany(c => c.ArmorProficiencyChoices, ch =>
            ch.ToJson("ArmorOptions"));

        builder.OwnsMany(c => c.WeaponTypeProficiencyChoices, ch =>
            ch.ToJson("WeaponTypeOptions"));

        builder.OwnsMany(c => c.WeaponCategoryProficiencyChoices, ch =>
                ch.ToJson("WeaponCategoryOptions"));

        builder.OwnsMany(c => c.LanguageChoices, ch =>
                ch.ToJson("LanguageOptions"));

        builder.OwnsMany(c => c.ToolProficiencyChoices, ch =>
                ch.ToJson("ToolOptions"));

        builder.OwnsMany(c => c.SkillProficiencyChoices, ch =>
                ch.ToJson("SkillOptions"));
    }

    public static void ConfigureProficiencies(this EntityTypeBuilder<Character> builder)
    {
        builder.HasMany(c => c.ReadySpells)
            .WithMany()
            .UsingEntity(j => j.ToTable("CharacterSpells"));
        
        builder.HasMany(c => c.AbilityScores)
            .WithMany()
            .UsingEntity(j => j.ToTable("CharacterAbilityScores"));
        
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