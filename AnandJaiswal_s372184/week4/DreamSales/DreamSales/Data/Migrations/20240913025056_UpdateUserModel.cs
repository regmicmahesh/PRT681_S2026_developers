using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DreamSales.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_AspNetUsers_UserId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_AspNetUsers_SellerId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_SellerId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_UserId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "CartItems");

            migrationBuilder.RenameColumn(
                name: "QuantityAvailable",
                table: "Items",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "Items",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "CartItemId",
                table: "CartItems",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "SellerId",
                table: "Items",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CartItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "CartUsername",
                table: "CartItems",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "CartItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Username);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartUsername",
                table: "CartItems",
                column: "CartUsername");

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

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_CartUsername",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "CartUsername",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "CartItems");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Items",
                newName: "QuantityAvailable");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Items",
                newName: "ItemId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CartItems",
                newName: "CartItemId");

            migrationBuilder.AlterColumn<string>(
                name: "SellerId",
                table: "Items",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Items",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Items",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CartItems",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdded",
                table: "CartItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Items_SellerId",
                table: "Items",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_UserId",
                table: "CartItems",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_AspNetUsers_UserId",
                table: "CartItems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_AspNetUsers_SellerId",
                table: "Items",
                column: "SellerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
