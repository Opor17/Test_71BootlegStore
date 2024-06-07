using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _71BootlegStore.Migrations
{
    public partial class addProductCartDetailTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductId",
                table: "CartDetail",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "CartDetail");
        }
    }
}
