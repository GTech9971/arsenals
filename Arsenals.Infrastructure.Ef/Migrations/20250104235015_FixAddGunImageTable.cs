using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Arsenals.Infrastructure.Ef.Migrations
{
    /// <inheritdoc />
    public partial class FixAddGunImageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "gun_image_id",
                schema: "arsenals",
                table: "guns",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "銃画像外部キー");

            migrationBuilder.CreateIndex(
                name: "IX_guns_gun_image_id",
                schema: "arsenals",
                table: "guns",
                column: "gun_image_id");

            migrationBuilder.AddForeignKey(
                name: "FK_guns_gun_images_gun_image_id",
                schema: "arsenals",
                table: "guns",
                column: "gun_image_id",
                principalSchema: "arsenals",
                principalTable: "gun_images",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_guns_gun_images_gun_image_id",
                schema: "arsenals",
                table: "guns");

            migrationBuilder.DropIndex(
                name: "IX_guns_gun_image_id",
                schema: "arsenals",
                table: "guns");

            migrationBuilder.DropColumn(
                name: "gun_image_id",
                schema: "arsenals",
                table: "guns");
        }
    }
}
