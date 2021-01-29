using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aluguru.Marketplace.Data.Migrations
{
    public partial class Added_Product_InvalidDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlockedDates",
                table: "Product");

            migrationBuilder.AddColumn<Guid>(
                name: "InvalidDatesId",
                table: "Product",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "InvalidDates",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: true),
                    ProductId = table.Column<Guid>(nullable: false),
                    Days = table.Column<string>(nullable: true),
                    Dates = table.Column<string>(nullable: true),
                    Periods = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvalidDates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvalidDates_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvalidDates_ProductId",
                table: "InvalidDates",
                column: "ProductId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvalidDates");

            migrationBuilder.DropColumn(
                name: "InvalidDatesId",
                table: "Product");

            migrationBuilder.AddColumn<string>(
                name: "BlockedDates",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
