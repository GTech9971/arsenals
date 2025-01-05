using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Arsenals.Infrastructure.Ef.Migrations
{
    /// <inheritdoc />
    public partial class AddGunImageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "gun_images",
                schema: "arsenals",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, comment: "主キー")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    gun_id = table.Column<string>(type: "text", nullable: false, comment: "銃の外部キー")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gun_images", x => x.id);
                    table.ForeignKey(
                        name: "FK_gun_images_guns_gun_id",
                        column: x => x.gun_id,
                        principalSchema: "arsenals",
                        principalTable: "guns",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_gun_images_gun_id",
                schema: "arsenals",
                table: "gun_images",
                column: "gun_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "gun_images",
                schema: "arsenals");
        }
    }
}
