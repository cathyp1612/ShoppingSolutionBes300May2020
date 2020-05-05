using Microsoft.EntityFrameworkCore.Migrations;

namespace ShoppingApi.Migrations
{
    public partial class PurchasedBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PurchasedBy",
                table: "ShoppingItems",
                nullable: true);

            migrationBuilder.InsertData(
                table: "ShoppingItems",
                columns: new[] { "Id", "Description", "Purchased", "PurchasedBy", "PurchasedFrom" },
                values: new object[] { 1, "Beer", false, null, null });

            migrationBuilder.InsertData(
                table: "ShoppingItems",
                columns: new[] { "Id", "Description", "Purchased", "PurchasedBy", "PurchasedFrom" },
                values: new object[] { 2, "Toilet Paper", true, null, "Acme" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ShoppingItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ShoppingItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "PurchasedBy",
                table: "ShoppingItems");
        }
    }
}
