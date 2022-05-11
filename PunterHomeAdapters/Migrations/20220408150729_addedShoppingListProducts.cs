using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace PunterHomeAdapters.Migrations
{
    public partial class addedShoppingListProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShoppingListProductItem",
                columns: table => new
                {
                    ShoppingListAggregateId = table.Column<Guid>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<Guid>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    MeasurementType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingListProductItem", x => new { x.ShoppingListAggregateId, x.Id });
                    table.ForeignKey(
                        name: "FK_ShoppingListProductItem_ShoppingLists_ShoppingListAggregate~",
                        column: x => x.ShoppingListAggregateId,
                        principalTable: "ShoppingLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShoppingListProductItem");
        }
    }
}
