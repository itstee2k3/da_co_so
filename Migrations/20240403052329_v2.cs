using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace do_an.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_CategoryFrameColors_IdCategoryFrameColor",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_CategoryFrameStyles_IdCategoryFrameStyle",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_CategoryIrisColors_IdCategoryIrisColor",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_CategoryMaterials_IdCategoryMaterial",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_CategoryOrigins_IdCategoryOrigin",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_CategoryPrices_IdCategoryPrice",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_CategorySexes_IdCategorySex",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "IdCategorySex",
                table: "Products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "IdCategoryPrice",
                table: "Products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "IdCategoryOrigin",
                table: "Products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "IdCategoryMaterial",
                table: "Products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "IdCategoryIrisColor",
                table: "Products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "IdCategoryFrameStyle",
                table: "Products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "IdCategoryFrameColor",
                table: "Products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CategoryFrameColors_IdCategoryFrameColor",
                table: "Products",
                column: "IdCategoryFrameColor",
                principalTable: "CategoryFrameColors",
                principalColumn: "IdCategory");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CategoryFrameStyles_IdCategoryFrameStyle",
                table: "Products",
                column: "IdCategoryFrameStyle",
                principalTable: "CategoryFrameStyles",
                principalColumn: "IdCategory");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CategoryIrisColors_IdCategoryIrisColor",
                table: "Products",
                column: "IdCategoryIrisColor",
                principalTable: "CategoryIrisColors",
                principalColumn: "IdCategory");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CategoryMaterials_IdCategoryMaterial",
                table: "Products",
                column: "IdCategoryMaterial",
                principalTable: "CategoryMaterials",
                principalColumn: "IdCategory");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CategoryOrigins_IdCategoryOrigin",
                table: "Products",
                column: "IdCategoryOrigin",
                principalTable: "CategoryOrigins",
                principalColumn: "IdCategory");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CategoryPrices_IdCategoryPrice",
                table: "Products",
                column: "IdCategoryPrice",
                principalTable: "CategoryPrices",
                principalColumn: "IdCategory");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CategorySexes_IdCategorySex",
                table: "Products",
                column: "IdCategorySex",
                principalTable: "CategorySexes",
                principalColumn: "IdCategory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_CategoryFrameColors_IdCategoryFrameColor",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_CategoryFrameStyles_IdCategoryFrameStyle",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_CategoryIrisColors_IdCategoryIrisColor",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_CategoryMaterials_IdCategoryMaterial",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_CategoryOrigins_IdCategoryOrigin",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_CategoryPrices_IdCategoryPrice",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_CategorySexes_IdCategorySex",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "IdCategorySex",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdCategoryPrice",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdCategoryOrigin",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdCategoryMaterial",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdCategoryIrisColor",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdCategoryFrameStyle",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdCategoryFrameColor",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CategoryFrameColors_IdCategoryFrameColor",
                table: "Products",
                column: "IdCategoryFrameColor",
                principalTable: "CategoryFrameColors",
                principalColumn: "IdCategory",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CategoryFrameStyles_IdCategoryFrameStyle",
                table: "Products",
                column: "IdCategoryFrameStyle",
                principalTable: "CategoryFrameStyles",
                principalColumn: "IdCategory",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CategoryIrisColors_IdCategoryIrisColor",
                table: "Products",
                column: "IdCategoryIrisColor",
                principalTable: "CategoryIrisColors",
                principalColumn: "IdCategory",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CategoryMaterials_IdCategoryMaterial",
                table: "Products",
                column: "IdCategoryMaterial",
                principalTable: "CategoryMaterials",
                principalColumn: "IdCategory",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CategoryOrigins_IdCategoryOrigin",
                table: "Products",
                column: "IdCategoryOrigin",
                principalTable: "CategoryOrigins",
                principalColumn: "IdCategory",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CategoryPrices_IdCategoryPrice",
                table: "Products",
                column: "IdCategoryPrice",
                principalTable: "CategoryPrices",
                principalColumn: "IdCategory",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CategorySexes_IdCategorySex",
                table: "Products",
                column: "IdCategorySex",
                principalTable: "CategorySexes",
                principalColumn: "IdCategory",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
