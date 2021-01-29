using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aluguru.Marketplace.Data.Migrations
{
    public partial class RentPeriod_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Product");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Voucher",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DailyRentPrice",
                table: "Product",
                type: "decimal",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PeriodRentPrices",
                table: "Product",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SellPrice",
                table: "Product",
                type: "decimal",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FieldName",
                table: "CustomField",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RentPeriod",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(type: "varchar(250)", nullable: false),
                    Days = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentPeriod", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RentPeriod");

            migrationBuilder.DropColumn(
                name: "DailyRentPrice",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "PeriodRentPrices",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "SellPrice",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "FieldName",
                table: "CustomField");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Voucher",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Product",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
