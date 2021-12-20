using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PunterHomeAdapters.Migrations
{
    public partial class addedtextshoppinglistitem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShoppingListTextItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FkShoppingListId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsChecked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingListTextItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingListTextItems_ShoppingLists_FkShoppingListId",
                        column: x => x.FkShoppingListId,
                        principalTable: "ShoppingLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListTextItems_FkShoppingListId",
                table: "ShoppingListTextItems",
                column: "FkShoppingListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShoppingListTextItems");
        }
    }
}
