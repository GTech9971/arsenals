using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Arsenals.Infrastructure.Ef.Migrations
{
    /// <inheritdoc />
    public partial class FixAddGunImageTableIII : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_guns_gun_images_gun_image_id",
                schema: "arsenals",
                table: "guns");

            migrationBuilder.AlterColumn<int>(
                name: "gun_image_id",
                schema: "arsenals",
                table: "guns",
                type: "integer",
                nullable: true,
                comment: "銃画像外部キー",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "銃画像外部キー");

            migrationBuilder.AddForeignKey(
                name: "FK_guns_gun_images_gun_image_id",
                schema: "arsenals",
                table: "guns",
                column: "gun_image_id",
                principalSchema: "arsenals",
                principalTable: "gun_images",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_guns_gun_images_gun_image_id",
                schema: "arsenals",
                table: "guns");

            migrationBuilder.AlterColumn<int>(
                name: "gun_image_id",
                schema: "arsenals",
                table: "guns",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "銃画像外部キー",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "銃画像外部キー");

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
    }
}
