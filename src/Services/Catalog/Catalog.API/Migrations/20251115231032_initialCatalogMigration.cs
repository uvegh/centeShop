using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Catalog.API.Migrations
{
    /// <inheritdoc />
    public partial class initialCatalogMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    StockQuantity = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CreatedAt", "Name", "Price", "StockQuantity", "UpdatedAT" },
                values: new object[,]
                {
                    { new Guid("28d9b656-d3ab-4064-8409-9fe1833a80f2"), new DateTime(2025, 11, 15, 23, 10, 30, 797, DateTimeKind.Utc).AddTicks(2193), "Lenovo", 999.99m, 10, new DateTime(2025, 11, 15, 23, 10, 30, 797, DateTimeKind.Utc).AddTicks(2197) },
                    { new Guid("4b1056e5-040f-44f6-acdc-a778001759d6"), new DateTime(2025, 11, 15, 23, 10, 30, 797, DateTimeKind.Utc).AddTicks(2217), "Iphone 15", 800.77m, 10, new DateTime(2025, 11, 15, 23, 10, 30, 797, DateTimeKind.Utc).AddTicks(2218) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
