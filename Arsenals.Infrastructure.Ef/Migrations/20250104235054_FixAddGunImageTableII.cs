using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Arsenals.Infrastructure.Ef.Migrations
{
    /// <inheritdoc />
    public partial class FixAddGunImageTableII : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_gun_images_guns_gun_id",
                schema: "arsenals",
                table: "gun_images");

            migrationBuilder.DropIndex(
                name: "IX_gun_images_gun_id",
                schema: "arsenals",
                table: "gun_images");

            migrationBuilder.DropColumn(
                name: "gun_id",
                schema: "arsenals",
                table: "gun_images");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "gun_id",
                schema: "arsenals",
                table: "gun_images",
                type: "text",
                nullable: false,
                defaultValue: "",
                comment: "銃の外部キー");

            migrationBuilder.CreateIndex(
                name: "IX_gun_images_gun_id",
                schema: "arsenals",
                table: "gun_images",
                column: "gun_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_gun_images_guns_gun_id",
                schema: "arsenals",
                table: "gun_images",
                column: "gun_id",
                principalSchema: "arsenals",
                principalTable: "guns",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
