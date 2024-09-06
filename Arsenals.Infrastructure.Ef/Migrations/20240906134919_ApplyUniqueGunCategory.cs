using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Arsenals.Infrastructure.Ef.Migrations
{
    /// <inheritdoc />
    public partial class ApplyUniqueGunCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_gun_categories_name",
                table: "gun_categories");

            migrationBuilder.CreateIndex(
                name: "IX_gun_categories_name",
                table: "gun_categories",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_gun_categories_name",
                table: "gun_categories");

            migrationBuilder.CreateIndex(
                name: "IX_gun_categories_name",
                table: "gun_categories",
                column: "name");
        }
    }
}
