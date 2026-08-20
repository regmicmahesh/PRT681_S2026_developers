using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoard.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyOwner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Companies",
                type: "TEXT",
                maxLength: 450,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_OwnerId",
                table: "Companies",
                column: "OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Companies_OwnerId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Companies");
        }
    }
}
