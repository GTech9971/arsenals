using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Arsenals.Infrastructure.Ef.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "arsenals");

            migrationBuilder.CreateTable(
                name: "bullets",
                schema: "arsenals",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false, comment: "主キー"),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "弾丸名"),
                    damage = table.Column<int>(type: "integer", nullable: false, comment: "ダメージ")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bullets", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "gun_categories",
                schema: "arsenals",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false, comment: "主キー"),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "カテゴリー名")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gun_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "guns",
                schema: "arsenals",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false, comment: "主キー"),
                    name = table.Column<string>(type: "text", nullable: false, comment: "銃の名称"),
                    capacity = table.Column<int>(type: "integer", nullable: false, comment: "装弾数"),
                    gun_category_id = table.Column<string>(type: "text", nullable: false, comment: "銃のカテゴリー外部キー")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_guns", x => x.id);
                    table.ForeignKey(
                        name: "FK_guns_gun_categories_gun_category_id",
                        column: x => x.gun_category_id,
                        principalSchema: "arsenals",
                        principalTable: "gun_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BulletDataGunData",
                schema: "arsenals",
                columns: table => new
                {
                    BulletDataListId = table.Column<string>(type: "text", nullable: false),
                    GunDataListId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BulletDataGunData", x => new { x.BulletDataListId, x.GunDataListId });
                    table.ForeignKey(
                        name: "FK_BulletDataGunData_bullets_BulletDataListId",
                        column: x => x.BulletDataListId,
                        principalSchema: "arsenals",
                        principalTable: "bullets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BulletDataGunData_guns_GunDataListId",
                        column: x => x.GunDataListId,
                        principalSchema: "arsenals",
                        principalTable: "guns",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BulletDataGunData_GunDataListId",
                schema: "arsenals",
                table: "BulletDataGunData",
                column: "GunDataListId");

            migrationBuilder.CreateIndex(
                name: "IX_bullets_name",
                schema: "arsenals",
                table: "bullets",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_gun_categories_name",
                schema: "arsenals",
                table: "gun_categories",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_guns_gun_category_id",
                schema: "arsenals",
                table: "guns",
                column: "gun_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_guns_name",
                schema: "arsenals",
                table: "guns",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BulletDataGunData",
                schema: "arsenals");

            migrationBuilder.DropTable(
                name: "bullets",
                schema: "arsenals");

            migrationBuilder.DropTable(
                name: "guns",
                schema: "arsenals");

            migrationBuilder.DropTable(
                name: "gun_categories",
                schema: "arsenals");
        }
    }
}
