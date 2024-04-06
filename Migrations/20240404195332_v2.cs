using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace do_an.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_CategoryBrands_IdCategoryBrand",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "IdCategoryBrand",
                table: "Products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CategoryBrands_IdCategoryBrand",
                table: "Products",
                column: "IdCategoryBrand",
                principalTable: "CategoryBrands",
                principalColumn: "IdCategory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_CategoryBrands_IdCategoryBrand",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "IdCategoryBrand",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CategoryBrands_IdCategoryBrand",
                table: "Products",
                column: "IdCategoryBrand",
                principalTable: "CategoryBrands",
                principalColumn: "IdCategory",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
