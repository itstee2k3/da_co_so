using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace do_an.Migrations
{
    public partial class model3d : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Model3D",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Model3D",
                table: "Products");
        }
    }
}
