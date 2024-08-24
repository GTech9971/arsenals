using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Arsenals.Infrastructure.Ef.Migrations
{
    /// <inheritdoc />
    public partial class FIXBullet_Relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bullets_guns_gun_id",
                table: "bullets");

            migrationBuilder.DropIndex(
                name: "IX_bullets_gun_id",
                table: "bullets");

            migrationBuilder.DropColumn(
                name: "gun_id",
                table: "bullets");

            migrationBuilder.CreateTable(
                name: "BulletDataGunData",
                columns: table => new
                {
                    BulletDataListId = table.Column<int>(type: "integer", nullable: false),
                    GunDataListId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BulletDataGunData", x => new { x.BulletDataListId, x.GunDataListId });
                    table.ForeignKey(
                        name: "FK_BulletDataGunData_bullets_BulletDataListId",
                        column: x => x.BulletDataListId,
                        principalTable: "bullets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BulletDataGunData_guns_GunDataListId",
                        column: x => x.GunDataListId,
                        principalTable: "guns",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BulletDataGunData_GunDataListId",
                table: "BulletDataGunData",
                column: "GunDataListId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BulletDataGunData");

            migrationBuilder.AddColumn<int>(
                name: "gun_id",
                table: "bullets",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_bullets_gun_id",
                table: "bullets",
                column: "gun_id");

            migrationBuilder.AddForeignKey(
                name: "FK_bullets_guns_gun_id",
                table: "bullets",
                column: "gun_id",
                principalTable: "guns",
                principalColumn: "id");
        }
    }
}
