using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Catalog.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                    { new Guid("1c7967fb-25c5-4c71-9b12-09596b946e94"), new DateTime(2025, 11, 8, 14, 39, 56, 608, DateTimeKind.Utc).AddTicks(9746), "Lenovo", 999.99m, 10, new DateTime(2025, 11, 8, 14, 39, 56, 608, DateTimeKind.Utc).AddTicks(9750) },
                    { new Guid("2d3f45f1-823e-4773-a454-59f5dea3cc7c"), new DateTime(2025, 11, 8, 14, 39, 56, 608, DateTimeKind.Utc).AddTicks(9754), "Iphone 15", 800.77m, 10, new DateTime(2025, 11, 8, 14, 39, 56, 608, DateTimeKind.Utc).AddTicks(9754) }
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
