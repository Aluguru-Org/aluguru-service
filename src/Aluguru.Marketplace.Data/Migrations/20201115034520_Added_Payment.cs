using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aluguru.Marketplace.Data.Migrations
{
    public partial class Added_Payment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderItemStatus",
                table: "OrderItem",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "RentStartDate",
                table: "OrderItem",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    OrderId = table.Column<Guid>(nullable: false),
                    PaymentMethod = table.Column<int>(nullable: false),
                    InvoiceId = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    Pdf = table.Column<string>(nullable: true),
                    Identification = table.Column<string>(nullable: true),
                    Paid = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropColumn(
                name: "OrderItemStatus",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "RentStartDate",
                table: "OrderItem");
        }
    }
}
