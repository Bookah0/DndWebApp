using Api.Models.Characters;
using Api.Models.Features;
using Api.Models.Items;
using Api.Models.Spells;
using Api.Models.World;
using Api.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Character> Characters { get; set; }
    public DbSet<Inventory> Inventories { get; set; }

    public DbSet<Ability> AbilityScores { get; set; }
    public DbSet<AbilityValue> AbilityValues { get; set; }
    public DbSet<Skill> Skills { get; set; }

    public DbSet<Alignment> Alignments { get; set; }
    public DbSet<Language> Languages { get; set; }

    public DbSet<BaseClass> Classes { get; set; }
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

    public DbSet<AbilityIncreaseChoice> AbilityIncreaseChoices { get; set; }
    public DbSet<SkillProficiencyChoice> SkillProficiencyChoices { get; set; }
    public DbSet<ToolProficiencyChoice> ToolProficiencyChoices { get; set; }
    public DbSet<LanguageChoice> LanguageChoices { get; set; }
    public DbSet<ArmorProficiencyChoice> ArmorProficiencyChoices { get; set; }
    public DbSet<WeaponCategoryProficiencyChoice> WeaponCategoryProficiencyChoices { get; set; }
    public DbSet<WeaponTypeProficiencyChoice> WeaponTypeProficiencyChoices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Feature>().ConfigureProficiencyChoices();
        modelBuilder.Entity<Character>().ConfigureProficiencies();

        modelBuilder.Entity<ClassLevel>()
            .OwnsMany(c => c.ClassSpecificSlotsAtLevel, slot =>
            {
                slot.HasKey(s => s.Id);
                slot.WithOwner().HasForeignKey("ClassLevelId");
            });

        modelBuilder.Entity<BaseClass>()
            .OwnsMany(c => c.StartingEquipmentChoices, opt =>
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
    public static void ConfigureProficiencyChoices(this EntityTypeBuilder<Feature> builder)
    {
        builder.HasMany(f => f.AbilityIncreases)
            .WithMany()
            .UsingEntity(j => j.ToTable("AbilityIncreases"));
        
        builder.HasMany(c => c.SpellsGained)
            .WithMany()
            .UsingEntity(j => j.ToTable("SpellsGained"));

        builder.HasMany(f => f.AbilityIncreaseChoices)
            .WithOne()
            .HasForeignKey(a => a.FeatureId);

        builder.HasMany(f => f.SkillProficiencyChoices)
            .WithOne()
            .HasForeignKey(s => s.FeatureId);

        builder.HasMany(f => f.ToolProficiencyChoices)
            .WithOne()
            .HasForeignKey(t => t.FeatureId);

        builder.HasMany(f => f.LanguageChoices)
            .WithOne()  
            .HasForeignKey(l => l.FeatureId);
        
        builder.HasMany(f => f.ArmorProficiencyChoices)
            .WithOne()
            .HasForeignKey(a => a.FeatureId);

        builder.HasMany(f => f.WeaponCategoryProficiencyChoices)
            .WithOne()
            .HasForeignKey(wc => wc.FeatureId);

        builder.HasMany(f => f.WeaponTypeProficiencyChoices)
            .WithOne()
            .HasForeignKey(wt => wt.FeatureId);
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