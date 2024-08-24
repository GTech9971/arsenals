using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Arsenals.Infrastructure.Ef.Migrations
{
    /// <inheritdoc />
    public partial class DelBullet_GunFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bullets_guns_gun_id",
                table: "bullets");

            migrationBuilder.DropIndex(
                name: "IX_guns_name",
                table: "guns");

            migrationBuilder.DropIndex(
                name: "IX_bullets_name",
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

            migrationBuilder.CreateIndex(
                name: "IX_guns_name",
                table: "guns",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bullets_name",
                table: "bullets",
                column: "name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_bullets_guns_GunDataId",
                table: "bullets",
                column: "GunDataId",
                principalTable: "guns",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bullets_guns_GunDataId",
                table: "bullets");

            migrationBuilder.DropIndex(
                name: "IX_guns_name",
                table: "guns");

            migrationBuilder.DropIndex(
                name: "IX_bullets_name",
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

            migrationBuilder.CreateIndex(
                name: "IX_guns_name",
                table: "guns",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_bullets_name",
                table: "bullets",
                column: "name");

            migrationBuilder.AddForeignKey(
                name: "FK_bullets_guns_gun_id",
                table: "bullets",
                column: "gun_id",
                principalTable: "guns",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
