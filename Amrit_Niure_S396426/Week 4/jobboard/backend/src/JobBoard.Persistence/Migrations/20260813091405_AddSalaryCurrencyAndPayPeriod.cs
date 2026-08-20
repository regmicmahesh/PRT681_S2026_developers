using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoard.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSalaryCurrencyAndPayPeriod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SalaryPayPeriod",
                table: "Jobs",
                type: "TEXT",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalaryPayPeriod",
                table: "Jobs");
        }
    }
}
