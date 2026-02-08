using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Backgrounds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    StartingCurrency_Brass = table.Column<int>(type: "integer", nullable: false),
                    StartingCurrency_Copper = table.Column<int>(type: "integer", nullable: false),
                    StartingCurrency_Silver = table.Column<int>(type: "integer", nullable: false),
                    StartingCurrency_Gold = table.Column<int>(type: "integer", nullable: false),
                    StartingCurrency_Platinum = table.Column<int>(type: "integer", nullable: false),
                    StartingCurrency_Electrum = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsHomebrew = table.Column<bool>(type: "boolean", nullable: false),
                    IsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    CloningAllowed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Backgrounds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Class",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    HitDie = table.Column<int>(type: "integer", nullable: false),
                    SpellcastingAbilityId = table.Column<int>(type: "integer", nullable: true),
                    SpellcastingAbility = table.Column<string>(type: "text", nullable: true),
                    Discriminator = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false),
                    ParentClassId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsHomebrew = table.Column<bool>(type: "boolean", nullable: false),
                    IsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    CloningAllowed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Class", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Class_Class_ParentClassId",
                        column: x => x.ParentClassId,
                        principalTable: "Class",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "Species",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Info_General = table.Column<string>(type: "text", nullable: false),
                    Info_Aging = table.Column<string>(type: "text", nullable: false),
                    Info_CommonAlignment = table.Column<string>(type: "text", nullable: false),
                    Info_Size = table.Column<string>(type: "text", nullable: false),
                    Info_Languages = table.Column<string>(type: "text", nullable: false),
                    Speed = table.Column<int>(type: "integer", nullable: false),
                    Size = table.Column<string>(type: "text", nullable: false),
                    Discriminator = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    ParentRaceId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsHomebrew = table.Column<bool>(type: "boolean", nullable: false),
                    IsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    CloningAllowed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Species", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Species_Species_ParentRaceId",
                        column: x => x.ParentRaceId,
                        principalTable: "Species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Spells",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    EffectsAtHigherLevels = table.Column<string>(type: "text", nullable: false),
                    Duration = table.Column<string>(type: "text", nullable: false),
                    DurationValue = table.Column<int>(type: "integer", nullable: true),
                    CastingTime = table.Column<string>(type: "text", nullable: false),
                    CastingTimeValue = table.Column<int>(type: "integer", nullable: true),
                    ReactionCondition = table.Column<string>(type: "text", nullable: false),
                    MagicSchool = table.Column<string>(type: "text", nullable: false),
                    DamageRoll = table.Column<string>(type: "text", nullable: false),
                    DamageTypes = table.Column<string[]>(type: "text[]", nullable: false),
                    SpellTypes = table.Column<string[]>(type: "text[]", nullable: false),
                    SpellTargeting_TargetType = table.Column<string>(type: "text", nullable: false),
                    SpellTargeting_Range = table.Column<string>(type: "text", nullable: false),
                    SpellTargeting_RangeValue = table.Column<int>(type: "integer", nullable: true),
                    SpellTargeting_ShapeType = table.Column<string>(type: "text", nullable: true),
                    SpellTargeting_ShapeWidth = table.Column<string>(type: "text", nullable: true),
                    SpellTargeting_ShapeLength = table.Column<string>(type: "text", nullable: true),
                    CastingRequirements_Verbal = table.Column<bool>(type: "boolean", nullable: false),
                    CastingRequirements_Somatic = table.Column<bool>(type: "boolean", nullable: false),
                    CastingRequirements_Materials = table.Column<string>(type: "text", nullable: false),
                    CastingRequirements_MaterialCost = table.Column<int>(type: "integer", nullable: false),
                    CastingRequirements_MaterialsConsumed = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsHomebrew = table.Column<bool>(type: "boolean", nullable: false),
                    IsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    CloningAllowed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spells", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Backgrounds_CloningHistory",
                columns: table => new
                {
                    BackgroundId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClonedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClonedBy = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Backgrounds_CloningHistory", x => new { x.BackgroundId, x.Id });
                    table.ForeignKey(
                        name: "FK_Backgrounds_CloningHistory_Backgrounds_BackgroundId",
                        column: x => x.BackgroundId,
                        principalTable: "Backgrounds",
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
                name: "Class_CloningHistory",
                columns: table => new
                {
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClonedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClonedBy = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Class_CloningHistory", x => new { x.ClassId, x.Id });
                    table.ForeignKey(
                        name: "FK_Class_CloningHistory_Class_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Class",
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
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsHomebrew = table.Column<bool>(type: "boolean", nullable: false),
                    IsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    CloningAllowed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassLevels_Class_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Class",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StartingEquipmentChoice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: false),
                    NumberOfChoices = table.Column<int>(type: "integer", nullable: true),
                    ClassId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StartingEquipmentChoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StartingEquipmentChoice_Class_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Class",
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
                    Slot = table.Column<string>(type: "text", nullable: false)
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
                    Categories = table.Column<string[]>(type: "text[]", nullable: false),
                    Rarity = table.Column<string>(type: "text", nullable: false),
                    RequiresAttunement = table.Column<bool>(type: "boolean", nullable: false),
                    Weight = table.Column<int>(type: "integer", nullable: true),
                    Value = table.Column<int>(type: "integer", nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    BackgroundId = table.Column<int>(type: "integer", nullable: true),
                    BaseClassId = table.Column<int>(type: "integer", nullable: true),
                    Discriminator = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    InventoryId = table.Column<int>(type: "integer", nullable: true),
                    ArmorCategory = table.Column<string>(type: "text", nullable: true),
                    BaseArmorClass = table.Column<int>(type: "integer", nullable: true),
                    PlusDexMod = table.Column<bool>(type: "boolean", nullable: true),
                    ModCap = table.Column<int>(type: "integer", nullable: true),
                    StrengthScoreRequired = table.Column<int>(type: "integer", nullable: true),
                    StealthDisadvantage = table.Column<bool>(type: "boolean", nullable: true),
                    ToolCategory = table.Column<string>(type: "text", nullable: true),
                    WeaponCategory = table.Column<string>(type: "text", nullable: true),
                    WeaponType = table.Column<string>(type: "text", nullable: true),
                    Slot = table.Column<string>(type: "text", nullable: true),
                    Properties = table.Column<string[]>(type: "text[]", nullable: true),
                    DamageTypes = table.Column<string[]>(type: "text[]", nullable: true),
                    DamageDice = table.Column<string>(type: "text", nullable: true),
                    Range = table.Column<int>(type: "integer", nullable: true),
                    VersatileDamageDice = table.Column<string>(type: "text", nullable: true),
                    LongRange = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsHomebrew = table.Column<bool>(type: "boolean", nullable: false),
                    IsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    CloningAllowed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Backgrounds_BackgroundId",
                        column: x => x.BackgroundId,
                        principalTable: "Backgrounds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Items_Class_BaseClassId",
                        column: x => x.BaseClassId,
                        principalTable: "Class",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Items_Inventories_InventoryId",
                        column: x => x.InventoryId,
                        principalTable: "Inventories",
                        principalColumn: "Id");
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
                    RaceId = table.Column<int>(type: "integer", nullable: false),
                    SubraceId = table.Column<int>(type: "integer", nullable: true),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    SubClassId = table.Column<int>(type: "integer", nullable: true),
                    BackgroundId = table.Column<int>(type: "integer", nullable: false),
                    Info_AlignmentId = table.Column<int>(type: "integer", nullable: true),
                    Info_PersonalityTraits = table.Column<string>(type: "text", nullable: false),
                    Info_Ideals = table.Column<string>(type: "text", nullable: false),
                    Info_Bonds = table.Column<string>(type: "text", nullable: false),
                    Info_Flaws = table.Column<string>(type: "text", nullable: false),
                    Info_Age = table.Column<int>(type: "integer", nullable: true),
                    Info_Height = table.Column<int>(type: "integer", nullable: true),
                    Info_Weight = table.Column<int>(type: "integer", nullable: true),
                    Info_Eyes = table.Column<string>(type: "text", nullable: false),
                    Info_Skin = table.Column<string>(type: "text", nullable: false),
                    Info_Hair = table.Column<string>(type: "text", nullable: false),
                    Info_AlliesAndOrganizations = table.Column<string>(type: "text", nullable: false),
                    Info_Backstory = table.Column<string>(type: "text", nullable: false),
                    Info_CharacterPictureUrl = table.Column<string>(type: "text", nullable: true),
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
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsHomebrew = table.Column<bool>(type: "boolean", nullable: false),
                    IsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    CloningAllowed = table.Column<bool>(type: "boolean", nullable: false),
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
                        name: "FK_Characters_Backgrounds_BackgroundId",
                        column: x => x.BackgroundId,
                        principalTable: "Backgrounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Characters_Class_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Class",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Characters_Class_SubClassId",
                        column: x => x.SubClassId,
                        principalTable: "Class",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Characters_Inventories_InventoryId1",
                        column: x => x.InventoryId1,
                        principalTable: "Inventories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Characters_Species_RaceId",
                        column: x => x.RaceId,
                        principalTable: "Species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Characters_Species_SubraceId",
                        column: x => x.SubraceId,
                        principalTable: "Species",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Species_CloningHistory",
                columns: table => new
                {
                    SpeciesId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClonedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClonedBy = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Species_CloningHistory", x => new { x.SpeciesId, x.Id });
                    table.ForeignKey(
                        name: "FK_Species_CloningHistory_Species_SpeciesId",
                        column: x => x.SpeciesId,
                        principalTable: "Species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        name: "FK_SpellClasses_Class_ClassesId",
                        column: x => x.ClassesId,
                        principalTable: "Class",
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
                name: "Spells_CloningHistory",
                columns: table => new
                {
                    SpellId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClonedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClonedBy = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spells_CloningHistory", x => new { x.SpellId, x.Id });
                    table.ForeignKey(
                        name: "FK_Spells_CloningHistory_Spells_SpellId",
                        column: x => x.SpellId,
                        principalTable: "Spells",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassLevels_ClassSlotsAtLevel",
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
                    table.PrimaryKey("PK_ClassLevels_ClassSlotsAtLevel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassLevels_ClassSlotsAtLevel_ClassLevels_ClassLevelId",
                        column: x => x.ClassLevelId,
                        principalTable: "ClassLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassLevels_CloningHistory",
                columns: table => new
                {
                    ClassLevelId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClonedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClonedBy = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassLevels_CloningHistory", x => new { x.ClassLevelId, x.Id });
                    table.ForeignKey(
                        name: "FK_ClassLevels_CloningHistory_ClassLevels_ClassLevelId",
                        column: x => x.ClassLevelId,
                        principalTable: "ClassLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Items_CloningHistory",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClonedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClonedBy = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items_CloningHistory", x => new { x.ItemId, x.Id });
                    table.ForeignKey(
                        name: "FK_Items_CloningHistory_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StartingEquipmentOption",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StartingEquipmentChoiceId = table.Column<int>(type: "integer", nullable: false),
                    EquipmentId = table.Column<int>(type: "integer", nullable: true),
                    AnyOfArmorCategory = table.Column<string>(type: "text", nullable: true),
                    AnyOfWeaponCategory = table.Column<string>(type: "text", nullable: true),
                    AnyOfWeaponType = table.Column<string>(type: "text", nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StartingEquipmentOption", x => new { x.StartingEquipmentChoiceId, x.Id });
                    table.ForeignKey(
                        name: "FK_StartingEquipmentOption_Items_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Items",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StartingEquipmentOption_StartingEquipmentChoice_StartingEqu~",
                        column: x => x.StartingEquipmentChoiceId,
                        principalTable: "StartingEquipmentChoice",
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
                name: "Characters_CloningHistory",
                columns: table => new
                {
                    CharacterId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClonedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClonedBy = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters_CloningHistory", x => new { x.CharacterId, x.Id });
                    table.ForeignKey(
                        name: "FK_Characters_CloningHistory_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
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
                name: "Feature",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    DamageResistanceGained = table.Column<string[]>(type: "text[]", nullable: false),
                    DamageImmunityGained = table.Column<string[]>(type: "text[]", nullable: false),
                    DamageWeaknessGained = table.Column<string[]>(type: "text[]", nullable: false),
                    ToolProficiencies = table.Column<string[]>(type: "text[]", nullable: false),
                    WeaponCategoryProficiencies = table.Column<string[]>(type: "text[]", nullable: false),
                    WeaponTypeProficiencies = table.Column<string[]>(type: "text[]", nullable: false),
                    ArmorProficiencies = table.Column<string[]>(type: "text[]", nullable: false),
                    BackgroundId = table.Column<int>(type: "integer", nullable: true),
                    Discriminator = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    BackgroundId1 = table.Column<int>(type: "integer", nullable: true),
                    LevelId = table.Column<int>(type: "integer", nullable: true),
                    ClassId = table.Column<int>(type: "integer", nullable: true),
                    Prerequisite = table.Column<string>(type: "text", nullable: true),
                    FromClassId = table.Column<int>(type: "integer", nullable: true),
                    FromRaceId = table.Column<int>(type: "integer", nullable: true),
                    FromBackgroundId = table.Column<int>(type: "integer", nullable: true),
                    CharacterId = table.Column<int>(type: "integer", nullable: true),
                    Trait_FromRaceId = table.Column<int>(type: "integer", nullable: true),
                    RaceId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsHomebrew = table.Column<bool>(type: "boolean", nullable: false),
                    IsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    CloningAllowed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feature", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Feature_Backgrounds_BackgroundId",
                        column: x => x.BackgroundId,
                        principalTable: "Backgrounds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Feature_Backgrounds_BackgroundId1",
                        column: x => x.BackgroundId1,
                        principalTable: "Backgrounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Feature_Backgrounds_FromBackgroundId",
                        column: x => x.FromBackgroundId,
                        principalTable: "Backgrounds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Feature_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Feature_ClassLevels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "ClassLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Feature_Class_FromClassId",
                        column: x => x.FromClassId,
                        principalTable: "Class",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Feature_Species_FromRaceId",
                        column: x => x.FromRaceId,
                        principalTable: "Species",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Feature_Species_Trait_FromRaceId",
                        column: x => x.Trait_FromRaceId,
                        principalTable: "Species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbilityIncreaseChoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: false),
                    FeatureId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbilityIncreaseChoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbilityIncreaseChoices_Feature_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Feature",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbilityScores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ShortName = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    FeatureId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbilityScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbilityScores_Feature_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Feature",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ArmorProficiencyChoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Options = table.Column<string[]>(type: "text[]", nullable: false),
                    FeatureId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArmorProficiencyChoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArmorProficiencyChoices_Feature_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Feature",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Feature_CloningHistory",
                columns: table => new
                {
                    FeatureId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClonedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClonedBy = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feature_CloningHistory", x => new { x.FeatureId, x.Id });
                    table.ForeignKey(
                        name: "FK_Feature_CloningHistory_Feature_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Feature",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LanguageChoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: false),
                    FeatureId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageChoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LanguageChoices_Feature_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Feature",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SkillProficiencyChoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: false),
                    FeatureId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillProficiencyChoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkillProficiencyChoices_Feature_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Feature",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpellsGained",
                columns: table => new
                {
                    FeatureId = table.Column<int>(type: "integer", nullable: false),
                    SpellsGainedId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpellsGained", x => new { x.FeatureId, x.SpellsGainedId });
                    table.ForeignKey(
                        name: "FK_SpellsGained_Feature_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Feature",
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
                name: "ToolProficiencyChoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Options = table.Column<string[]>(type: "text[]", nullable: false),
                    FeatureId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToolProficiencyChoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToolProficiencyChoices_Feature_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Feature",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeaponCategoryProficiencyChoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Options = table.Column<string[]>(type: "text[]", nullable: false),
                    FeatureId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeaponCategoryProficiencyChoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeaponCategoryProficiencyChoices_Feature_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Feature",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeaponTypeProficiencyChoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Options = table.Column<string[]>(type: "text[]", nullable: false),
                    FeatureId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeaponTypeProficiencyChoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeaponTypeProficiencyChoices_Feature_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Feature",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbilityValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AbilityId = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false),
                    AbilityIncreaseChoiceId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbilityValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbilityValues_AbilityIncreaseChoices_AbilityIncreaseChoiceId",
                        column: x => x.AbilityIncreaseChoiceId,
                        principalTable: "AbilityIncreaseChoices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AbilityValues_AbilityScores_AbilityId",
                        column: x => x.AbilityId,
                        principalTable: "AbilityScores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Family = table.Column<string>(type: "text", nullable: false),
                    Script = table.Column<string>(type: "text", nullable: true),
                    TypicalSpeakers = table.Column<string>(type: "text", nullable: true),
                    FeatureId = table.Column<int>(type: "integer", nullable: true),
                    LanguageChoiceId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsHomebrew = table.Column<bool>(type: "boolean", nullable: false),
                    IsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    CloningAllowed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Languages_Feature_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Feature",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Languages_LanguageChoices_LanguageChoiceId",
                        column: x => x.LanguageChoiceId,
                        principalTable: "LanguageChoices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    AbilityId = table.Column<int>(type: "integer", nullable: false),
                    FeatureId = table.Column<int>(type: "integer", nullable: true),
                    SkillProficiencyChoiceId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsHomebrew = table.Column<bool>(type: "boolean", nullable: false),
                    IsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    CloningAllowed = table.Column<bool>(type: "boolean", nullable: false)
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
                    table.ForeignKey(
                        name: "FK_Skills_Feature_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Feature",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Skills_SkillProficiencyChoices_SkillProficiencyChoiceId",
                        column: x => x.SkillProficiencyChoiceId,
                        principalTable: "SkillProficiencyChoices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AbilityIncreases",
                columns: table => new
                {
                    AbilityIncreasesId = table.Column<int>(type: "integer", nullable: false),
                    FeatureId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbilityIncreases", x => new { x.AbilityIncreasesId, x.FeatureId });
                    table.ForeignKey(
                        name: "FK_AbilityIncreases_AbilityValues_AbilityIncreasesId",
                        column: x => x.AbilityIncreasesId,
                        principalTable: "AbilityValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbilityIncreases_Feature_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Feature",
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
                        name: "FK_CharacterAbilityScores_AbilityValues_AbilityScoresId",
                        column: x => x.AbilityScoresId,
                        principalTable: "AbilityValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterAbilityScores_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Languages_CloningHistory",
                columns: table => new
                {
                    LanguageId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClonedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClonedBy = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages_CloningHistory", x => new { x.LanguageId, x.Id });
                    table.ForeignKey(
                        name: "FK_Languages_CloningHistory_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Skills_CloningHistory",
                columns: table => new
                {
                    SkillId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClonedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClonedBy = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills_CloningHistory", x => new { x.SkillId, x.Id });
                    table.ForeignKey(
                        name: "FK_Skills_CloningHistory_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbilityIncreaseChoices_FeatureId",
                table: "AbilityIncreaseChoices",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_AbilityIncreases_FeatureId",
                table: "AbilityIncreases",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_AbilityScores_FeatureId",
                table: "AbilityScores",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_AbilityValues_AbilityId",
                table: "AbilityValues",
                column: "AbilityId");

            migrationBuilder.CreateIndex(
                name: "IX_AbilityValues_AbilityIncreaseChoiceId",
                table: "AbilityValues",
                column: "AbilityIncreaseChoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ArmorProficiencyChoices_FeatureId",
                table: "ArmorProficiencyChoices",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

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
                name: "IX_Class_ParentClassId",
                table: "Class",
                column: "ParentClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassLevels_ClassId",
                table: "ClassLevels",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassLevels_ClassSlotsAtLevel_ClassLevelId",
                table: "ClassLevels_ClassSlotsAtLevel",
                column: "ClassLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Feature_BackgroundId",
                table: "Feature",
                column: "BackgroundId");

            migrationBuilder.CreateIndex(
                name: "IX_Feature_BackgroundId1",
                table: "Feature",
                column: "BackgroundId1");

            migrationBuilder.CreateIndex(
                name: "IX_Feature_CharacterId",
                table: "Feature",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Feature_FromBackgroundId",
                table: "Feature",
                column: "FromBackgroundId");

            migrationBuilder.CreateIndex(
                name: "IX_Feature_FromClassId",
                table: "Feature",
                column: "FromClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Feature_FromRaceId",
                table: "Feature",
                column: "FromRaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Feature_LevelId",
                table: "Feature",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Feature_Trait_FromRaceId",
                table: "Feature",
                column: "Trait_FromRaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_BackgroundId",
                table: "Items",
                column: "BackgroundId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_BaseClassId",
                table: "Items",
                column: "BaseClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_InventoryId",
                table: "Items",
                column: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageChoices_FeatureId",
                table: "LanguageChoices",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_FeatureId",
                table: "Languages",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_LanguageChoiceId",
                table: "Languages",
                column: "LanguageChoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillProficiencyChoices_FeatureId",
                table: "SkillProficiencyChoices",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_AbilityId",
                table: "Skills",
                column: "AbilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_FeatureId",
                table: "Skills",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_SkillProficiencyChoiceId",
                table: "Skills",
                column: "SkillProficiencyChoiceId");

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
                name: "IX_StartingEquipmentChoice_ClassId",
                table: "StartingEquipmentChoice",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_StartingEquipmentOption_EquipmentId",
                table: "StartingEquipmentOption",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_StartingItemOption_BackgroundId",
                table: "StartingItemOption",
                column: "BackgroundId");

            migrationBuilder.CreateIndex(
                name: "IX_ToolProficiencyChoices_FeatureId",
                table: "ToolProficiencyChoices",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_WeaponCategoryProficiencyChoices_FeatureId",
                table: "WeaponCategoryProficiencyChoices",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_WeaponTypeProficiencyChoices_FeatureId",
                table: "WeaponTypeProficiencyChoices",
                column: "FeatureId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbilityIncreases");

            migrationBuilder.DropTable(
                name: "Alignments");

            migrationBuilder.DropTable(
                name: "ArmorProficiencyChoices");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Backgrounds_CloningHistory");

            migrationBuilder.DropTable(
                name: "CharacterAbilityScores");

            migrationBuilder.DropTable(
                name: "Characters_CloningHistory");

            migrationBuilder.DropTable(
                name: "Characters_CurrentClassSlots");

            migrationBuilder.DropTable(
                name: "CharacterSpells");

            migrationBuilder.DropTable(
                name: "Class_CloningHistory");

            migrationBuilder.DropTable(
                name: "ClassLevels_ClassSlotsAtLevel");

            migrationBuilder.DropTable(
                name: "ClassLevels_CloningHistory");

            migrationBuilder.DropTable(
                name: "EquipmentSlot");

            migrationBuilder.DropTable(
                name: "Feature_CloningHistory");

            migrationBuilder.DropTable(
                name: "Items_CloningHistory");

            migrationBuilder.DropTable(
                name: "Languages_CloningHistory");

            migrationBuilder.DropTable(
                name: "Skills_CloningHistory");

            migrationBuilder.DropTable(
                name: "Species_CloningHistory");

            migrationBuilder.DropTable(
                name: "SpellClasses");

            migrationBuilder.DropTable(
                name: "Spells_CloningHistory");

            migrationBuilder.DropTable(
                name: "SpellsGained");

            migrationBuilder.DropTable(
                name: "StartingEquipmentOption");

            migrationBuilder.DropTable(
                name: "StartingItemOption");

            migrationBuilder.DropTable(
                name: "ToolActivity");

            migrationBuilder.DropTable(
                name: "ToolProficiencyChoices");

            migrationBuilder.DropTable(
                name: "ToolProperty");

            migrationBuilder.DropTable(
                name: "WeaponCategoryProficiencyChoices");

            migrationBuilder.DropTable(
                name: "WeaponTypeProficiencyChoices");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "AbilityValues");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "Spells");

            migrationBuilder.DropTable(
                name: "StartingEquipmentChoice");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "AbilityIncreaseChoices");

            migrationBuilder.DropTable(
                name: "LanguageChoices");

            migrationBuilder.DropTable(
                name: "AbilityScores");

            migrationBuilder.DropTable(
                name: "SkillProficiencyChoices");

            migrationBuilder.DropTable(
                name: "Feature");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "ClassLevels");

            migrationBuilder.DropTable(
                name: "Backgrounds");

            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropTable(
                name: "Species");

            migrationBuilder.DropTable(
                name: "Class");
        }
    }
}
