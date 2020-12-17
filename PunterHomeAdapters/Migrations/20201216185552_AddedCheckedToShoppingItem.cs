using Microsoft.EntityFrameworkCore.Migrations;

namespace PunterHomeAdapters.Migrations
{
    public partial class AddedCheckedToShoppingItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Checked",
                table: "ShoppingListItems",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Checked",
                table: "ShoppingListItems");
        }
    }
}
