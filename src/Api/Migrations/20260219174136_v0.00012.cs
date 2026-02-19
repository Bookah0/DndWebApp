using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class v000012 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToolActivity_Items_ToolId",
                table: "ToolActivity");

            migrationBuilder.DropForeignKey(
                name: "FK_ToolProperty_Items_ToolId",
                table: "ToolProperty");

            migrationBuilder.DropColumn(
                name: "ArmorCategory",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "BaseArmorClass",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "DamageDice",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "DamageTypes",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "LongRange",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ModCap",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "PlusDexMod",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Properties",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Range",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Slot",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "StealthDisadvantage",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "StrengthScoreRequired",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ToolCategory",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "VersatileDamageDice",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "WeaponCategory",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "WeaponType",
                table: "Items");

            migrationBuilder.CreateTable(
                name: "Armors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    ArmorCategory = table.Column<string>(type: "text", nullable: false),
                    BaseArmorClass = table.Column<int>(type: "integer", nullable: false),
                    PlusDexMod = table.Column<bool>(type: "boolean", nullable: false),
                    ModCap = table.Column<int>(type: "integer", nullable: true),
                    StrengthScoreRequired = table.Column<int>(type: "integer", nullable: true),
                    StealthDisadvantage = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Armors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Armors_Items_Id",
                        column: x => x.Id,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tools",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    ToolCategory = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tools_Items_Id",
                        column: x => x.Id,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Weapons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    WeaponCategory = table.Column<string>(type: "text", nullable: false),
                    WeaponType = table.Column<string>(type: "text", nullable: false),
                    Slot = table.Column<string>(type: "text", nullable: false),
                    Properties = table.Column<string[]>(type: "text[]", nullable: false),
                    DamageTypes = table.Column<string[]>(type: "text[]", nullable: false),
                    DamageDice = table.Column<string>(type: "text", nullable: false),
                    Range = table.Column<int>(type: "integer", nullable: false),
                    VersatileDamageDice = table.Column<string>(type: "text", nullable: true),
                    LongRange = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weapons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Weapons_Items_Id",
                        column: x => x.Id,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ToolActivity_Tools_ToolId",
                table: "ToolActivity",
                column: "ToolId",
                principalTable: "Tools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ToolProperty_Tools_ToolId",
                table: "ToolProperty",
                column: "ToolId",
                principalTable: "Tools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToolActivity_Tools_ToolId",
                table: "ToolActivity");

            migrationBuilder.DropForeignKey(
                name: "FK_ToolProperty_Tools_ToolId",
                table: "ToolProperty");

            migrationBuilder.DropTable(
                name: "Armors");

            migrationBuilder.DropTable(
                name: "Tools");

            migrationBuilder.DropTable(
                name: "Weapons");

            migrationBuilder.AddColumn<string>(
                name: "ArmorCategory",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BaseArmorClass",
                table: "Items",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DamageDice",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "DamageTypes",
                table: "Items",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Items",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "LongRange",
                table: "Items",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModCap",
                table: "Items",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PlusDexMod",
                table: "Items",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "Properties",
                table: "Items",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Range",
                table: "Items",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slot",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "StealthDisadvantage",
                table: "Items",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StrengthScoreRequired",
                table: "Items",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ToolCategory",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VersatileDamageDice",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WeaponCategory",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WeaponType",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ToolActivity_Items_ToolId",
                table: "ToolActivity",
                column: "ToolId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ToolProperty_Items_ToolId",
                table: "ToolProperty",
                column: "ToolId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
