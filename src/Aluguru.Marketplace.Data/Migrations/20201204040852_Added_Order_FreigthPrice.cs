using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aluguru.Marketplace.Data.Migrations
{
    public partial class Added_Order_FreigthPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "OrderItem",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "FreigthPrice",
                table: "OrderItem",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ProductUri",
                table: "OrderItem",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RentDays",
                table: "OrderItem",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalFreigthPrice",
                table: "Order",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "FreigthPrice",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "ProductUri",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "RentDays",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "TotalFreigthPrice",
                table: "Order");
        }
    }
}
