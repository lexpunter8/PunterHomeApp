using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PunterHomeAdapters.Migrations
{
    public partial class changeshoppinglist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeShoppingListItem_ShoppingListItems_ShoppingListItemId",
                table: "RecipeShoppingListItem");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingListItems_Products_ProductId",
                table: "ShoppingListItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingListItems_ShoppingLists_ShoppingListId",
                table: "ShoppingListItems");

            migrationBuilder.DropColumn(
                name: "MeasurementAmount",
                table: "ShoppingListItems");

            migrationBuilder.DropColumn(
                name: "MeasurementType",
                table: "ShoppingListItems");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "ShoppingListItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "ShoppingListId",
                table: "ShoppingListItems",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "ShoppingListItems",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ShoppingListItemInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ShoppingListItemId = table.Column<Guid>(nullable: false),
                    MeasurementType = table.Column<int>(nullable: false),
                    MeasurementAmount = table.Column<int>(nullable: false),
                    Reason = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingListItemInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingListItemInfos_ShoppingListItems_ShoppingListItemId",
                        column: x => x.ShoppingListItemId,
                        principalTable: "ShoppingListItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListItemInfos_ShoppingListItemId",
                table: "ShoppingListItemInfos",
                column: "ShoppingListItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeShoppingListItem_ShoppingListItemInfos_ShoppingListIt~",
                table: "RecipeShoppingListItem",
                column: "ShoppingListItemId",
                principalTable: "ShoppingListItemInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingListItems_Products_ProductId",
                table: "ShoppingListItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingListItems_ShoppingLists_ShoppingListId",
                table: "ShoppingListItems",
                column: "ShoppingListId",
                principalTable: "ShoppingLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeShoppingListItem_ShoppingListItemInfos_ShoppingListIt~",
                table: "RecipeShoppingListItem");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingListItems_Products_ProductId",
                table: "ShoppingListItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingListItems_ShoppingLists_ShoppingListId",
                table: "ShoppingListItems");

            migrationBuilder.DropTable(
                name: "ShoppingListItemInfos");

            migrationBuilder.AlterColumn<Guid>(
                name: "ShoppingListId",
                table: "ShoppingListItems",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "ShoppingListItems",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<int>(
                name: "MeasurementAmount",
                table: "ShoppingListItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MeasurementType",
                table: "ShoppingListItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Reason",
                table: "ShoppingListItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeShoppingListItem_ShoppingListItems_ShoppingListItemId",
                table: "RecipeShoppingListItem",
                column: "ShoppingListItemId",
                principalTable: "ShoppingListItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingListItems_Products_ProductId",
                table: "ShoppingListItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingListItems_ShoppingLists_ShoppingListId",
                table: "ShoppingListItems",
                column: "ShoppingListId",
                principalTable: "ShoppingLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
