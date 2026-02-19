using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class v010010 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_EquipmentSlot_EquipmentId",
                table: "EquipmentSlot",
                column: "EquipmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentSlot_Items_EquipmentId",
                table: "EquipmentSlot",
                column: "EquipmentId",
                principalTable: "Items",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentSlot_Items_EquipmentId",
                table: "EquipmentSlot");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentSlot_EquipmentId",
                table: "EquipmentSlot");
        }
    }
}
