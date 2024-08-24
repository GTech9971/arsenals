using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Arsenals.Infrastructure.Ef.Migrations
{
    /// <inheritdoc />
    public partial class FIXBullet_GunFK : Migration
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

            migrationBuilder.AddForeignKey(
                name: "FK_bullets_guns_gun_id",
                table: "bullets",
                column: "gun_id",
                principalTable: "guns",
                principalColumn: "id");
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

            migrationBuilder.AddForeignKey(
                name: "FK_bullets_guns_GunDataId",
                table: "bullets",
                column: "GunDataId",
                principalTable: "guns",
                principalColumn: "id");
        }
    }
}
