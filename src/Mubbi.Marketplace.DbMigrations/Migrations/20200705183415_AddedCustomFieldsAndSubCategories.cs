using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mubbi.Marketplace.Data.Migrations
{
    public partial class AddedCustomFieldsAndSubCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Product");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Product",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(250)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Product",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(1000)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "Product",
                nullable: true,
                defaultValue: new DateTime(2020, 7, 5, 18, 34, 14, 690, DateTimeKind.Utc).AddTicks(9471),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Product",
                nullable: false,
                defaultValue: new DateTime(2020, 7, 5, 18, 34, 14, 690, DateTimeKind.Utc).AddTicks(7755),
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrls",
                table: "Product",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "SubCategoryId",
                table: "Product",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MainCategoryId",
                table: "Category",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomField",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2020, 7, 5, 18, 34, 14, 674, DateTimeKind.Utc).AddTicks(2193)),
                    DateUpdated = table.Column<DateTime>(nullable: true, defaultValue: new DateTime(2020, 7, 5, 18, 34, 14, 680, DateTimeKind.Utc).AddTicks(5688)),
                    FieldType = table.Column<int>(nullable: false),
                    ValueAsString = table.Column<string>(nullable: true),
                    ValueAsInt = table.Column<int>(nullable: true),
                    ValueAsOptions = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomField", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomField_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_SubCategoryId",
                table: "Product",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Category_MainCategoryId",
                table: "Category",
                column: "MainCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomField_ProductId",
                table: "CustomField",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Category_MainCategoryId",
                table: "Category",
                column: "MainCategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Category_SubCategoryId",
                table: "Product",
                column: "SubCategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Category_MainCategoryId",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Category_SubCategoryId",
                table: "Product");

            migrationBuilder.DropTable(
                name: "CustomField");

            migrationBuilder.DropIndex(
                name: "IX_Product_SubCategoryId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Category_MainCategoryId",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "ImageUrls",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "SubCategoryId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "MainCategoryId",
                table: "Category");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Product",
                type: "varchar(250)",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Product",
                type: "varchar(1000)",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "Product",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValue: new DateTime(2020, 7, 5, 18, 34, 14, 690, DateTimeKind.Utc).AddTicks(9471));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Product",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 7, 5, 18, 34, 14, 690, DateTimeKind.Utc).AddTicks(7755));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Product",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Product",
                type: "varchar(250)",
                nullable: false,
                defaultValue: "");
        }
    }
}
