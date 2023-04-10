using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoAPI.Migrations
{
    /// <inheritdoc />
    public partial class CryptoKandles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CryptoKandles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    openTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    openPrice = table.Column<decimal>(type: "decimal(20,10)", precision: 20, scale: 10, nullable: false),
                    highPrice = table.Column<decimal>(type: "decimal(20,10)", precision: 20, scale: 10, nullable: false),
                    lowPrice = table.Column<decimal>(type: "decimal(20,10)", precision: 20, scale: 10, nullable: false),
                    closePrice = table.Column<decimal>(type: "decimal(20,10)", precision: 20, scale: 10, nullable: false),
                    volume = table.Column<decimal>(type: "decimal(20,10)", precision: 20, scale: 10, nullable: false),
                    closeTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    quoteVolume = table.Column<decimal>(type: "decimal(20,10)", precision: 20, scale: 10, nullable: false),
                    tradeCount = table.Column<decimal>(type: "decimal(20,10)", precision: 20, scale: 10, nullable: false),
                    takerBuyBaseVolume = table.Column<decimal>(type: "decimal(20,10)", precision: 20, scale: 10, nullable: false),
                    takerBuyQuoteVolume = table.Column<decimal>(type: "decimal(20,10)", precision: 20, scale: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CryptoKandles", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CryptoKandles");
        }
    }
}
