using Microsoft.EntityFrameworkCore.Migrations;

namespace Aluguru.Marketplace.Data.Migrations
{
    public partial class Removed_CustomField_ValueAsFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValueAsInt",
                table: "CustomField");

            migrationBuilder.DropColumn(
                name: "ValueAsString",
                table: "CustomField");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ValueAsInt",
                table: "CustomField",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ValueAsString",
                table: "CustomField",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
