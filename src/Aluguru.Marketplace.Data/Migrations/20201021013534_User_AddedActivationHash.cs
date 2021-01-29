using Microsoft.EntityFrameworkCore.Migrations;

namespace Aluguru.Marketplace.Data.Migrations
{
    public partial class User_AddedActivationHash : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActivationHash",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivationHash",
                table: "User");
        }
    }
}
