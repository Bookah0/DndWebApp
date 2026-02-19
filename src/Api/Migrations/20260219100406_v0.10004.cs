using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class v010004 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feature_Backgrounds_BackgroundId",
                table: "Feature");

            migrationBuilder.DropForeignKey(
                name: "FK_Feature_Backgrounds_BackgroundId1",
                table: "Feature");

            migrationBuilder.DropIndex(
                name: "IX_Feature_BackgroundId1",
                table: "Feature");

            migrationBuilder.DropColumn(
                name: "BackgroundId1",
                table: "Feature");

            migrationBuilder.AddForeignKey(
                name: "FK_Feature_Backgrounds_BackgroundId",
                table: "Feature",
                column: "BackgroundId",
                principalTable: "Backgrounds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feature_Backgrounds_BackgroundId",
                table: "Feature");

            migrationBuilder.AddColumn<int>(
                name: "BackgroundId1",
                table: "Feature",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Feature_BackgroundId1",
                table: "Feature",
                column: "BackgroundId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Feature_Backgrounds_BackgroundId",
                table: "Feature",
                column: "BackgroundId",
                principalTable: "Backgrounds",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Feature_Backgrounds_BackgroundId1",
                table: "Feature",
                column: "BackgroundId1",
                principalTable: "Backgrounds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
