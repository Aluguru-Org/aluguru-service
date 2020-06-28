using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mubbi.Marketplace.Data.Migrations
{
    public partial class Initial_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(type: "varchar(250)", nullable: false),
                    Code = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: true),
                    CategoryId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(type: "varchar(250)", nullable: false),
                    Description = table.Column<string>(type: "varchar(1000)", nullable: false),
                    ImageUrl = table.Column<string>(type: "varchar(250)", nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    StockQuantity = table.Column<int>(nullable: false),
                    RentType = table.Column<int>(nullable: false),
                    MinRentTime = table.Column<TimeSpan>(nullable: false),
                    MaxRentTime = table.Column<TimeSpan>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryId",
                table: "Product",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Category");
        }
    }
}
