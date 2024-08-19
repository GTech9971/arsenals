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
            migrationBuilder.CreateTable(
                name: "gun_categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gun_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "guns",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    capacity = table.Column<int>(type: "integer", nullable: false),
                    gun_category_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_guns", x => x.id);
                    table.ForeignKey(
                        name: "FK_guns_gun_categories_gun_category_id",
                        column: x => x.gun_category_id,
                        principalTable: "gun_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bullets",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    damage = table.Column<int>(type: "integer", nullable: false),
                    GunDataId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bullets", x => x.id);
                    table.ForeignKey(
                        name: "FK_bullets_guns_GunDataId",
                        column: x => x.GunDataId,
                        principalTable: "guns",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_bullets_GunDataId",
                table: "bullets",
                column: "GunDataId");

            migrationBuilder.CreateIndex(
                name: "IX_bullets_name",
                table: "bullets",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_gun_categories_name",
                table: "gun_categories",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_guns_gun_category_id",
                table: "guns",
                column: "gun_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_guns_name",
                table: "guns",
                column: "name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bullets");

            migrationBuilder.DropTable(
                name: "guns");

            migrationBuilder.DropTable(
                name: "gun_categories");
        }
    }
}
