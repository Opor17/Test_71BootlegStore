using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _71BootlegStore.Migrations
{
    public partial class addTrackingOrderTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Tracking",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tracking",
                table: "Orders");
        }
    }
}
