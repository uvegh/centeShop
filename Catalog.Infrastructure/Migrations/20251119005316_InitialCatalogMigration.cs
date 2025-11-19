using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Catalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCatalogMigration : Migration
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
                    { new Guid("35c23cd6-666d-492c-b972-b0b17ea7c36a"), new DateTime(2025, 11, 19, 0, 53, 15, 821, DateTimeKind.Utc).AddTicks(6604), "Lenovo", 999.99m, 10, new DateTime(2025, 11, 19, 0, 53, 15, 821, DateTimeKind.Utc).AddTicks(6607) },
                    { new Guid("b938e2d3-4f45-4519-b1b2-7afceac841ff"), new DateTime(2025, 11, 19, 0, 53, 15, 821, DateTimeKind.Utc).AddTicks(6610), "Iphone 15", 800.77m, 10, new DateTime(2025, 11, 19, 0, 53, 15, 821, DateTimeKind.Utc).AddTicks(6611) }
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
