using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class v010003 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Inventories_InventoryId1",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentSlot_Inventories_InventoryId",
                table: "EquipmentSlot");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Inventories_InventoryId",
                table: "Items");

            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropIndex(
                name: "IX_Items_InventoryId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Characters_InventoryId1",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "InventoryId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "InventoryId",
                table: "EquipmentSlot",
                newName: "InventoryCharacterId");

            migrationBuilder.RenameColumn(
                name: "InventoryId1",
                table: "Characters",
                newName: "Inventory_TotalWeight");

            migrationBuilder.RenameColumn(
                name: "InventoryId",
                table: "Characters",
                newName: "Inventory_MaxWeight");

            migrationBuilder.AddColumn<bool>(
                name: "Stackable",
                table: "Items",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Inventory_AttunedItems",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Inventory_Currency_Brass",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Inventory_Currency_Copper",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Inventory_Currency_Electrum",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Inventory_Currency_Gold",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Inventory_Currency_Platinum",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Inventory_Currency_Silver",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "InventoryItem",
                columns: table => new
                {
                    InventoryCharacterId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItem", x => new { x.InventoryCharacterId, x.Id });
                    table.ForeignKey(
                        name: "FK_InventoryItem_Characters_InventoryCharacterId",
                        column: x => x.InventoryCharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentSlot_Characters_InventoryCharacterId",
                table: "EquipmentSlot",
                column: "InventoryCharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentSlot_Characters_InventoryCharacterId",
                table: "EquipmentSlot");

            migrationBuilder.DropTable(
                name: "InventoryItem");

            migrationBuilder.DropColumn(
                name: "Stackable",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Inventory_AttunedItems",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Inventory_Currency_Brass",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Inventory_Currency_Copper",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Inventory_Currency_Electrum",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Inventory_Currency_Gold",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Inventory_Currency_Platinum",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Inventory_Currency_Silver",
                table: "Characters");

            migrationBuilder.RenameColumn(
                name: "InventoryCharacterId",
                table: "EquipmentSlot",
                newName: "InventoryId");

            migrationBuilder.RenameColumn(
                name: "Inventory_TotalWeight",
                table: "Characters",
                newName: "InventoryId1");

            migrationBuilder.RenameColumn(
                name: "Inventory_MaxWeight",
                table: "Characters",
                newName: "InventoryId");

            migrationBuilder.AddColumn<int>(
                name: "InventoryId",
                table: "Items",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AttunedItems = table.Column<int>(type: "integer", nullable: false),
                    CharacterId = table.Column<int>(type: "integer", nullable: false),
                    MaxWeight = table.Column<int>(type: "integer", nullable: false),
                    TotalWeight = table.Column<int>(type: "integer", nullable: false),
                    Currency_Brass = table.Column<int>(type: "integer", nullable: false),
                    Currency_Copper = table.Column<int>(type: "integer", nullable: false),
                    Currency_Electrum = table.Column<int>(type: "integer", nullable: false),
                    Currency_Gold = table.Column<int>(type: "integer", nullable: false),
                    Currency_Platinum = table.Column<int>(type: "integer", nullable: false),
                    Currency_Silver = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_InventoryId",
                table: "Items",
                column: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_InventoryId1",
                table: "Characters",
                column: "InventoryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Inventories_InventoryId1",
                table: "Characters",
                column: "InventoryId1",
                principalTable: "Inventories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentSlot_Inventories_InventoryId",
                table: "EquipmentSlot",
                column: "InventoryId",
                principalTable: "Inventories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Inventories_InventoryId",
                table: "Items",
                column: "InventoryId",
                principalTable: "Inventories",
                principalColumn: "Id");
        }
    }
}
