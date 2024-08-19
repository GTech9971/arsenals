using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Arsenals.Infrastructure.Ef.Migrations
{
    /// <inheritdoc />
    public partial class FixBulletFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bullets_guns_GunDataId",
                table: "bullets");

            migrationBuilder.RenameColumn(
                name: "GunDataId",
                table: "bullets",
                newName: "gun_id");

            migrationBuilder.RenameIndex(
                name: "IX_bullets_GunDataId",
                table: "bullets",
                newName: "IX_bullets_gun_id");

            migrationBuilder.AlterColumn<int>(
                name: "gun_id",
                table: "bullets",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_bullets_guns_gun_id",
                table: "bullets",
                column: "gun_id",
                principalTable: "guns",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bullets_guns_gun_id",
                table: "bullets");

            migrationBuilder.RenameColumn(
                name: "gun_id",
                table: "bullets",
                newName: "GunDataId");

            migrationBuilder.RenameIndex(
                name: "IX_bullets_gun_id",
                table: "bullets",
                newName: "IX_bullets_GunDataId");

            migrationBuilder.AlterColumn<int>(
                name: "GunDataId",
                table: "bullets",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_bullets_guns_GunDataId",
                table: "bullets",
                column: "GunDataId",
                principalTable: "guns",
                principalColumn: "id");
        }
    }
}
