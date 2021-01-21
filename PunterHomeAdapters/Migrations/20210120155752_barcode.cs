using Microsoft.EntityFrameworkCore.Migrations;

namespace PunterHomeAdapters.Migrations
{
    public partial class barcode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "ProductQuantities",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "ProductQuantities");
        }
    }
}
