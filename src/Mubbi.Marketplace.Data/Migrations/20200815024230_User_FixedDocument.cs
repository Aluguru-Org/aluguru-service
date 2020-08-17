using Microsoft.EntityFrameworkCore.Migrations;

namespace Mubbi.Marketplace.Data.Migrations
{
    public partial class User_FixedDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Document_Number",
                table: "User",
                newName: "DocumentNumber");

            migrationBuilder.RenameColumn(
                name: "Document_DocumentType",
                table: "User",
                newName: "DocumentType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DocumentNumber",
                table: "User",
                newName: "Document_Number");

            migrationBuilder.RenameColumn(
                name: "DocumentType",
                table: "User",
                newName: "Document_DocumentType");
        }
    }
}
