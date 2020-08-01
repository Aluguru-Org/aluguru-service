using Microsoft.EntityFrameworkCore.Migrations;

namespace Mubbi.Marketplace.Data.Migrations
{
    public partial class Product_AddedRentType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MinNoticeRentDays",
                table: "Product",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RentType",
                table: "Product",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinNoticeRentDays",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "RentType",
                table: "Product");
        }
    }
}
