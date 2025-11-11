using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DndWebApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbilityScores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ShortName = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbilityScores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbilityValue",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AbilityId = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbilityValue", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AClass",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    HitDie = table.Column<string>(type: "text", nullable: false),
                    IsHomebrew = table.Column<bool>(type: "boolean", nullable: false),
                    SpellcastingAbilityId = table.Column<int>(type: "integer", nullable: true),
                    Discriminator = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    ClassId = table.Column<int>(type: "integer", nullable: true),
                    ParentClassId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AClass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AClass_AClass_ClassId",
                        column: x => x.ClassId,
                        principalTable: "AClass",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AClass_AClass_ParentClassId",
                        column: x => x.ParentClassId,
                        principalTable: "AClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Alignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Abbreviation = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alignments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Backgrounds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    IsHomebrew = table.Column<bool>(type: "boolean", nullable: false),
                    StartingCurrency_Brass = table.Column<int>(type: "integer", nullable: false),
                    StartingCurrency_Copper = table.Column<int>(type: "integer", nullable: false),
                    StartingCurrency_Silver = table.Column<int>(type: "integer", nullable: false),
                    StartingCurrency_Gold = table.Column<int>(type: "integer", nullable: false),
                    StartingCurrency_Platinum = table.Column<int>(type: "integer", nullable: false),
                    StartingCurrency_Electrum = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Backgrounds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CharacterId = table.Column<int>(type: "integer", nullable: false),
                    Currency_Brass = table.Column<int>(type: "integer", nullable: false),
                    Currency_Copper = table.Column<int>(type: "integer", nullable: false),
                    Currency_Silver = table.Column<int>(type: "integer", nullable: false),
                    Currency_Gold = table.Column<int>(type: "integer", nullable: false),
                    Currency_Platinum = table.Column<int>(type: "integer", nullable: false),
                    Currency_Electrum = table.Column<int>(type: "integer", nullable: false),
                    TotalWeight = table.Column<int>(type: "integer", nullable: false),
                    MaxWeight = table.Column<int>(type: "integer", nullable: false),
                    AttunedItems = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Family = table.Column<string>(type: "text", nullable: false),
                    Script = table.Column<string>(type: "text", nullable: false),
                    IsHomebrew = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Spells",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    IsHomebrew = table.Column<bool>(type: "boolean", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    EffectsAtHigherLevels = table.Column<string>(type: "text", nullable: false),
                    Duration = table.Column<int>(type: "integer", nullable: false),
                    DurationValue = table.Column<int>(type: "integer", nullable: false),
                    CastingTime = table.Column<int>(type: "integer", nullable: false),
                    CastingTimeValue = table.Column<int>(type: "integer", nullable: false),
                    ReactionCondition = table.Column<string>(type: "text", nullable: false),
                    MagicSchool = table.Column<int>(type: "integer", nullable: false),
                    DamageRoll = table.Column<string>(type: "text", nullable: false),
                    DamageTypes = table.Column<int[]>(type: "integer[]", nullable: false),
                    SpellTypes = table.Column<int[]>(type: "integer[]", nullable: false),
                    SpellTargeting_TargetType = table.Column<int>(type: "integer", nullable: false),
                    SpellTargeting_Range = table.Column<int>(type: "integer", nullable: false),
                    SpellTargeting_RangeValue = table.Column<int>(type: "integer", nullable: false),
                    SpellTargeting_ShapeType = table.Column<string>(type: "text", nullable: true),
                    SpellTargeting_ShapeWidth = table.Column<string>(type: "text", nullable: true),
                    SpellTargeting_ShapeLength = table.Column<string>(type: "text", nullable: true),
                    CastingRequirements_Verbal = table.Column<bool>(type: "boolean", nullable: false),
                    CastingRequirements_Somatic = table.Column<bool>(type: "boolean", nullable: false),
                    CastingRequirements_Materials = table.Column<string>(type: "text", nullable: true),
                    CastingRequirements_MaterialCost = table.Column<int>(type: "integer", nullable: true),
                    CastingRequirements_MaterialsConsumed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spells", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    AbilityId = table.Column<int>(type: "integer", nullable: false),
                    IsHomebrew = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skills_AbilityScores_AbilityId",
                        column: x => x.AbilityId,
                        principalTable: "AbilityScores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    ProficiencyBonus = table.Column<int>(type: "integer", nullable: false),
                    CantripsKnown = table.Column<int>(type: "integer", nullable: false),
                    SpellsKnown = table.Column<int>(type: "integer", nullable: false),
                    SpellSlots = table.Column<int[]>(type: "integer[]", nullable: true),
                    ClassId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassLevels_AClass_ClassId",
                        column: x => x.ClassId,
                        principalTable: "AClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StartingEquipmentOption",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: false),
                    OptionIds = table.Column<int[]>(type: "integer[]", nullable: false),
                    ClassId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StartingEquipmentOption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StartingEquipmentOption_AClass_ClassId",
                        column: x => x.ClassId,
                        principalTable: "AClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StartingItemOption",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ItemOptionIds = table.Column<int[]>(type: "integer[]", nullable: false),
                    BackgroundId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StartingItemOption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StartingItemOption_Backgrounds_BackgroundId",
                        column: x => x.BackgroundId,
                        principalTable: "Backgrounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentSlot",
                columns: table => new
                {
                    InventoryId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EquipmentId = table.Column<int>(type: "integer", nullable: true),
                    Slot = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentSlot", x => new { x.InventoryId, x.Id });
                    table.ForeignKey(
                        name: "FK_EquipmentSlot_Inventories_InventoryId",
                        column: x => x.InventoryId,
                        principalTable: "Inventories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Categories = table.Column<int[]>(type: "integer[]", nullable: false),
                    Rarity = table.Column<int>(type: "integer", nullable: true),
                    RequiresAttunement = table.Column<bool>(type: "boolean", nullable: false),
                    Weight = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    IsHomebrew = table.Column<bool>(type: "boolean", nullable: false),
                    BackgroundId = table.Column<int>(type: "integer", nullable: true),
                    ClassId = table.Column<int>(type: "integer", nullable: true),
                    Discriminator = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    InventoryId = table.Column<int>(type: "integer", nullable: true),
                    Category = table.Column<int>(type: "integer", nullable: true),
                    BaseArmorClass = table.Column<int>(type: "integer", nullable: true),
                    PlusDexMod = table.Column<bool>(type: "boolean", nullable: true),
                    ModCap = table.Column<int>(type: "integer", nullable: true),
                    StrengthScoreRequired = table.Column<int>(type: "integer", nullable: true),
                    StealthDisadvantage = table.Column<bool>(type: "boolean", nullable: true),
                    ToolType = table.Column<int>(type: "integer", nullable: true),
                    WeaponCategory = table.Column<int>(type: "integer", nullable: true),
                    WeaponType = table.Column<int>(type: "integer", nullable: true),
                    Properties = table.Column<int[]>(type: "integer[]", nullable: true),
                    DamageTypes = table.Column<int[]>(type: "integer[]", nullable: true),
                    DamageDice = table.Column<string>(type: "text", nullable: true),
                    Range = table.Column<int>(type: "integer", nullable: true),
                    VersitileDamageDice = table.Column<string>(type: "text", nullable: true),
                    LongRange = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_AClass_ClassId",
                        column: x => x.ClassId,
                        principalTable: "AClass",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Items_Backgrounds_BackgroundId",
                        column: x => x.BackgroundId,
                        principalTable: "Backgrounds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Items_Inventories_InventoryId",
                        column: x => x.InventoryId,
                        principalTable: "Inventories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SpellClasses",
                columns: table => new
                {
                    ClassesId = table.Column<int>(type: "integer", nullable: false),
                    SpellId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpellClasses", x => new { x.ClassesId, x.SpellId });
                    table.ForeignKey(
                        name: "FK_SpellClasses_AClass_ClassesId",
                        column: x => x.ClassesId,
                        principalTable: "AClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpellClasses_Spells_SpellId",
                        column: x => x.SpellId,
                        principalTable: "Spells",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassLevels_ClassSpecificSlotsAtLevel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    ClassLevelId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassLevels_ClassSpecificSlotsAtLevel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassLevels_ClassSpecificSlotsAtLevel_ClassLevels_ClassLeve~",
                        column: x => x.ClassLevelId,
                        principalTable: "ClassLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ToolActivity",
                columns: table => new
                {
                    ToolId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    SkillId = table.Column<int>(type: "integer", nullable: true),
                    AbilityId = table.Column<int>(type: "integer", nullable: true),
                    DC = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToolActivity", x => new { x.ToolId, x.Id });
                    table.ForeignKey(
                        name: "FK_ToolActivity_Items_ToolId",
                        column: x => x.ToolId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ToolProperty",
                columns: table => new
                {
                    ToolId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToolProperty", x => new { x.ToolId, x.Id });
                    table.ForeignKey(
                        name: "FK_ToolProperty_Items_ToolId",
                        column: x => x.ToolId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbilityIncreaseOptions",
                columns: table => new
                {
                    AFeature1Id = table.Column<int>(type: "integer", nullable: false),
                    AbilityIncreaseOptionsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbilityIncreaseOptions", x => new { x.AFeature1Id, x.AbilityIncreaseOptionsId });
                    table.ForeignKey(
                        name: "FK_AbilityIncreaseOptions_AbilityValue_AbilityIncreaseOptionsId",
                        column: x => x.AbilityIncreaseOptionsId,
                        principalTable: "AbilityValue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbilityIncreases",
                columns: table => new
                {
                    AFeatureId = table.Column<int>(type: "integer", nullable: false),
                    AbilityIncreasesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbilityIncreases", x => new { x.AFeatureId, x.AbilityIncreasesId });
                    table.ForeignKey(
                        name: "FK_AbilityIncreases_AbilityValue_AbilityIncreasesId",
                        column: x => x.AbilityIncreasesId,
                        principalTable: "AbilityValue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AFeature",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    IsHomebrew = table.Column<bool>(type: "boolean", nullable: false),
                    DamageResistanceGained = table.Column<int[]>(type: "integer[]", nullable: false),
                    DamageImmunityGained = table.Column<int[]>(type: "integer[]", nullable: false),
                    DamageWeaknessGained = table.Column<int[]>(type: "integer[]", nullable: false),
                    SavingThrowProficiencies = table.Column<int[]>(type: "integer[]", nullable: false),
                    SkillProficiencies = table.Column<int[]>(type: "integer[]", nullable: false),
                    WeaponCategoryProficiencies = table.Column<int[]>(type: "integer[]", nullable: false),
                    WeaponTypeProficiencies = table.Column<int[]>(type: "integer[]", nullable: false),
                    ArmorProficiencies = table.Column<int[]>(type: "integer[]", nullable: false),
                    ToolProficiencies = table.Column<int[]>(type: "integer[]", nullable: false),
                    Languages = table.Column<int[]>(type: "integer[]", nullable: false),
                    BackgroundId = table.Column<int>(type: "integer", nullable: true),
                    Discriminator = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    BackgroundId1 = table.Column<int>(type: "integer", nullable: true),
                    ClassLevelId = table.Column<int>(type: "integer", nullable: true),
                    Prerequisite = table.Column<string>(type: "text", nullable: true),
                    FromClassId = table.Column<int>(type: "integer", nullable: true),
                    FromRaceId = table.Column<int>(type: "integer", nullable: true),
                    FromBackgroundId = table.Column<int>(type: "integer", nullable: true),
                    Trait_FromRaceId = table.Column<int>(type: "integer", nullable: true),
                    RaceId = table.Column<int>(type: "integer", nullable: true),
                    ArmorOptions = table.Column<string>(type: "jsonb", nullable: true),
                    LanguageOptions = table.Column<string>(type: "jsonb", nullable: true),
                    SkillOptions = table.Column<string>(type: "jsonb", nullable: true),
                    ToolOptions = table.Column<string>(type: "jsonb", nullable: true),
                    WeaponCategoryOptions = table.Column<string>(type: "jsonb", nullable: true),
                    WeaponTypeOptions = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AFeature", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AFeature_AClass_FromClassId",
                        column: x => x.FromClassId,
                        principalTable: "AClass",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AFeature_Backgrounds_BackgroundId",
                        column: x => x.BackgroundId,
                        principalTable: "Backgrounds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AFeature_Backgrounds_BackgroundId1",
                        column: x => x.BackgroundId1,
                        principalTable: "Backgrounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AFeature_Backgrounds_FromBackgroundId",
                        column: x => x.FromBackgroundId,
                        principalTable: "Backgrounds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AFeature_ClassLevels_ClassLevelId",
                        column: x => x.ClassLevelId,
                        principalTable: "ClassLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpellsGained",
                columns: table => new
                {
                    AFeatureId = table.Column<int>(type: "integer", nullable: false),
                    SpellsGainedId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpellsGained", x => new { x.AFeatureId, x.SpellsGainedId });
                    table.ForeignKey(
                        name: "FK_SpellsGained_AFeature_AFeatureId",
                        column: x => x.AFeatureId,
                        principalTable: "AFeature",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpellsGained_Spells_SpellsGainedId",
                        column: x => x.SpellsGainedId,
                        principalTable: "Spells",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterAbilityScores",
                columns: table => new
                {
                    AbilityScoresId = table.Column<int>(type: "integer", nullable: false),
                    CharacterId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterAbilityScores", x => new { x.AbilityScoresId, x.CharacterId });
                    table.ForeignKey(
                        name: "FK_CharacterAbilityScores_AbilityValue_AbilityScoresId",
                        column: x => x.AbilityScoresId,
                        principalTable: "AbilityValue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    Experience = table.Column<int>(type: "integer", nullable: true),
                    PlayerName = table.Column<string>(type: "text", nullable: false),
                    TimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RaceId = table.Column<int>(type: "integer", nullable: false),
                    SubraceId = table.Column<int>(type: "integer", nullable: true),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    SubClassId = table.Column<int>(type: "integer", nullable: true),
                    BackgroundId = table.Column<int>(type: "integer", nullable: false),
                    CharacterDescription_AlignmentId = table.Column<int>(type: "integer", nullable: true),
                    CharacterDescription_PersonalityTraits = table.Column<string>(type: "text", nullable: false),
                    CharacterDescription_Ideals = table.Column<string>(type: "text", nullable: false),
                    CharacterDescription_Bonds = table.Column<string>(type: "text", nullable: false),
                    CharacterDescription_Flaws = table.Column<string>(type: "text", nullable: false),
                    CharacterDescription_Age = table.Column<int>(type: "integer", nullable: true),
                    CharacterDescription_Height = table.Column<int>(type: "integer", nullable: true),
                    CharacterDescription_Weight = table.Column<int>(type: "integer", nullable: true),
                    CharacterDescription_Eyes = table.Column<string>(type: "text", nullable: false),
                    CharacterDescription_Skin = table.Column<string>(type: "text", nullable: false),
                    CharacterDescription_Hair = table.Column<string>(type: "text", nullable: false),
                    CharacterDescription_AlliesAndOrganizations = table.Column<string>(type: "text", nullable: false),
                    CharacterDescription_Backstory = table.Column<string>(type: "text", nullable: false),
                    CharacterDescription_CharacterPictureUrl = table.Column<string>(type: "text", nullable: true),
                    InventoryId1 = table.Column<int>(type: "integer", nullable: false),
                    InventoryId = table.Column<int>(type: "integer", nullable: false),
                    CombatStats_MaxHP = table.Column<int>(type: "integer", nullable: false),
                    CombatStats_CurrentHP = table.Column<int>(type: "integer", nullable: false),
                    CombatStats_TempHP = table.Column<int>(type: "integer", nullable: false),
                    CombatStats_ArmorClass = table.Column<int>(type: "integer", nullable: false),
                    CombatStats_Initiative = table.Column<int>(type: "integer", nullable: false),
                    CombatStats_Speed = table.Column<int>(type: "integer", nullable: false),
                    CombatStats_MaxHitDice = table.Column<int>(type: "integer", nullable: false),
                    CombatStats_CurrentHitDice = table.Column<int>(type: "integer", nullable: false),
                    CurrentSpellSlots = table.Column<int[]>(type: "integer[]", nullable: true),
                    ProficiencyBonus = table.Column<int>(type: "integer", nullable: false),
                    ArmorProficiencies = table.Column<string>(type: "jsonb", nullable: true),
                    DamageAffinities = table.Column<string>(type: "jsonb", nullable: true),
                    Languages = table.Column<string>(type: "jsonb", nullable: true),
                    SavingThrows = table.Column<string>(type: "jsonb", nullable: true),
                    SkillProficiencies = table.Column<string>(type: "jsonb", nullable: true),
                    ToolProficiencies = table.Column<string>(type: "jsonb", nullable: true),
                    WeaponCategoryProficiencies = table.Column<string>(type: "jsonb", nullable: true),
                    WeaponTypeProficiencies = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Characters_AClass_ClassId",
                        column: x => x.ClassId,
                        principalTable: "AClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Characters_AClass_SubClassId",
                        column: x => x.SubClassId,
                        principalTable: "AClass",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Characters_Backgrounds_BackgroundId",
                        column: x => x.BackgroundId,
                        principalTable: "Backgrounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Characters_Inventories_InventoryId1",
                        column: x => x.InventoryId1,
                        principalTable: "Inventories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Characters_CurrentClassSlots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CharacterId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters_CurrentClassSlots", x => new { x.CharacterId, x.Id });
                    table.ForeignKey(
                        name: "FK_Characters_CurrentClassSlots_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterSpells",
                columns: table => new
                {
                    CharacterId = table.Column<int>(type: "integer", nullable: false),
                    ReadySpellsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterSpells", x => new { x.CharacterId, x.ReadySpellsId });
                    table.ForeignKey(
                        name: "FK_CharacterSpells_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterSpells_Spells_ReadySpellsId",
                        column: x => x.ReadySpellsId,
                        principalTable: "Spells",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Species",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    RaceDescription_GeneralDescription = table.Column<string>(type: "text", nullable: false),
                    RaceDescription_AgingDescription = table.Column<string>(type: "text", nullable: false),
                    RaceDescription_CommonAlignmentDescription = table.Column<string>(type: "text", nullable: false),
                    RaceDescription_SizeDescription = table.Column<string>(type: "text", nullable: false),
                    RaceDescription_LanguageDescription = table.Column<string>(type: "text", nullable: false),
                    IsHomebrew = table.Column<bool>(type: "boolean", nullable: false),
                    Speed = table.Column<int>(type: "integer", nullable: false),
                    Size = table.Column<int>(type: "integer", nullable: false),
                    Discriminator = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    CharacterId = table.Column<int>(type: "integer", nullable: true),
                    ParentRaceId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Species", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Species_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Species_Species_ParentRaceId",
                        column: x => x.ParentRaceId,
                        principalTable: "Species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbilityIncreaseOptions_AbilityIncreaseOptionsId",
                table: "AbilityIncreaseOptions",
                column: "AbilityIncreaseOptionsId");

            migrationBuilder.CreateIndex(
                name: "IX_AbilityIncreases_AbilityIncreasesId",
                table: "AbilityIncreases",
                column: "AbilityIncreasesId");

            migrationBuilder.CreateIndex(
                name: "IX_AClass_ClassId",
                table: "AClass",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_AClass_ParentClassId",
                table: "AClass",
                column: "ParentClassId");

            migrationBuilder.CreateIndex(
                name: "IX_AFeature_BackgroundId",
                table: "AFeature",
                column: "BackgroundId");

            migrationBuilder.CreateIndex(
                name: "IX_AFeature_BackgroundId1",
                table: "AFeature",
                column: "BackgroundId1");

            migrationBuilder.CreateIndex(
                name: "IX_AFeature_ClassLevelId",
                table: "AFeature",
                column: "ClassLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_AFeature_FromBackgroundId",
                table: "AFeature",
                column: "FromBackgroundId");

            migrationBuilder.CreateIndex(
                name: "IX_AFeature_FromClassId",
                table: "AFeature",
                column: "FromClassId");

            migrationBuilder.CreateIndex(
                name: "IX_AFeature_FromRaceId",
                table: "AFeature",
                column: "FromRaceId");

            migrationBuilder.CreateIndex(
                name: "IX_AFeature_Trait_FromRaceId",
                table: "AFeature",
                column: "Trait_FromRaceId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterAbilityScores_CharacterId",
                table: "CharacterAbilityScores",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_BackgroundId",
                table: "Characters",
                column: "BackgroundId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_ClassId",
                table: "Characters",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_InventoryId1",
                table: "Characters",
                column: "InventoryId1");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_RaceId",
                table: "Characters",
                column: "RaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_SubClassId",
                table: "Characters",
                column: "SubClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_SubraceId",
                table: "Characters",
                column: "SubraceId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterSpells_ReadySpellsId",
                table: "CharacterSpells",
                column: "ReadySpellsId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassLevels_ClassId",
                table: "ClassLevels",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassLevels_ClassSpecificSlotsAtLevel_ClassLevelId",
                table: "ClassLevels_ClassSpecificSlotsAtLevel",
                column: "ClassLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_BackgroundId",
                table: "Items",
                column: "BackgroundId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ClassId",
                table: "Items",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_InventoryId",
                table: "Items",
                column: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_AbilityId",
                table: "Skills",
                column: "AbilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Species_CharacterId",
                table: "Species",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Species_ParentRaceId",
                table: "Species",
                column: "ParentRaceId");

            migrationBuilder.CreateIndex(
                name: "IX_SpellClasses_SpellId",
                table: "SpellClasses",
                column: "SpellId");

            migrationBuilder.CreateIndex(
                name: "IX_SpellsGained_SpellsGainedId",
                table: "SpellsGained",
                column: "SpellsGainedId");

            migrationBuilder.CreateIndex(
                name: "IX_StartingEquipmentOption_ClassId",
                table: "StartingEquipmentOption",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_StartingItemOption_BackgroundId",
                table: "StartingItemOption",
                column: "BackgroundId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbilityIncreaseOptions_AFeature_AFeature1Id",
                table: "AbilityIncreaseOptions",
                column: "AFeature1Id",
                principalTable: "AFeature",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AbilityIncreases_AFeature_AFeatureId",
                table: "AbilityIncreases",
                column: "AFeatureId",
                principalTable: "AFeature",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AFeature_Species_FromRaceId",
                table: "AFeature",
                column: "FromRaceId",
                principalTable: "Species",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AFeature_Species_Trait_FromRaceId",
                table: "AFeature",
                column: "Trait_FromRaceId",
                principalTable: "Species",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterAbilityScores_Characters_CharacterId",
                table: "CharacterAbilityScores",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Species_RaceId",
                table: "Characters",
                column: "RaceId",
                principalTable: "Species",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Species_SubraceId",
                table: "Characters",
                column: "SubraceId",
                principalTable: "Species",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_AClass_ClassId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_AClass_SubClassId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Backgrounds_BackgroundId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Species_RaceId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Species_SubraceId",
                table: "Characters");

            migrationBuilder.DropTable(
                name: "AbilityIncreaseOptions");

            migrationBuilder.DropTable(
                name: "AbilityIncreases");

            migrationBuilder.DropTable(
                name: "Alignments");

            migrationBuilder.DropTable(
                name: "CharacterAbilityScores");

            migrationBuilder.DropTable(
                name: "Characters_CurrentClassSlots");

            migrationBuilder.DropTable(
                name: "CharacterSpells");

            migrationBuilder.DropTable(
                name: "ClassLevels_ClassSpecificSlotsAtLevel");

            migrationBuilder.DropTable(
                name: "EquipmentSlot");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "SpellClasses");

            migrationBuilder.DropTable(
                name: "SpellsGained");

            migrationBuilder.DropTable(
                name: "StartingEquipmentOption");

            migrationBuilder.DropTable(
                name: "StartingItemOption");

            migrationBuilder.DropTable(
                name: "ToolActivity");

            migrationBuilder.DropTable(
                name: "ToolProperty");

            migrationBuilder.DropTable(
                name: "AbilityValue");

            migrationBuilder.DropTable(
                name: "AbilityScores");

            migrationBuilder.DropTable(
                name: "AFeature");

            migrationBuilder.DropTable(
                name: "Spells");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "ClassLevels");

            migrationBuilder.DropTable(
                name: "AClass");

            migrationBuilder.DropTable(
                name: "Backgrounds");

            migrationBuilder.DropTable(
                name: "Species");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "Inventories");
        }
    }
}
