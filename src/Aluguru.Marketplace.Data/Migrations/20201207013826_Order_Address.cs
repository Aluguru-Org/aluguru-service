using Microsoft.EntityFrameworkCore.Migrations;

namespace Aluguru.Marketplace.Data.Migrations
{
    public partial class Order_Address : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductImageUrl",
                table: "OrderItem",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryAddress",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FreigthCalculated",
                table: "Order",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductImageUrl",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "DeliveryAddress",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "FreigthCalculated",
                table: "Order");
        }
    }
}
