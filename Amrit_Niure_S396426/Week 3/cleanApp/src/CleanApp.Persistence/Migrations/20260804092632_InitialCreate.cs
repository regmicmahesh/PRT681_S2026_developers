using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TodoItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TodoListId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Note = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    ReminderUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDone = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CompletedOnUtc = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TodoLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Colour = table.Column<string>(type: "TEXT", maxLength: 9, nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoLists", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_TodoListId",
                table: "TodoItems",
                column: "TodoListId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TodoItems");

            migrationBuilder.DropTable(
                name: "TodoLists");
        }
    }
}
