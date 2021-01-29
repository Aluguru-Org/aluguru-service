using Microsoft.EntityFrameworkCore.Migrations;

namespace Aluguru.Marketplace.Data.Migrations
{
    public partial class Added_Product_BlockedDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BlockedDates",
                table: "Product",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlockedDates",
                table: "Product");
        }
    }
}
