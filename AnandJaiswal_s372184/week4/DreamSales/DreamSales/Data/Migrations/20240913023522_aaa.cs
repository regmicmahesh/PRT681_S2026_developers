using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DreamSales.Data.Migrations
{
    /// <inheritdoc />
    public partial class aaa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Carts_CartUsername",
                table: "CartItems");

            migrationBuilder.AddColumn<string>(
                name: "SellerId",
                table: "Items",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "CartUsername",
                table: "CartItems",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "CartItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "CartItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Carts_CartUsername",
                table: "CartItems",
                column: "CartUsername",
                principalTable: "Carts",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Carts_CartUsername",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CartItems");

            migrationBuilder.AlterColumn<string>(
                name: "CartUsername",
                table: "CartItems",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Carts_CartUsername",
                table: "CartItems",
                column: "CartUsername",
                principalTable: "Carts",
                principalColumn: "Username");
        }
    }
}
