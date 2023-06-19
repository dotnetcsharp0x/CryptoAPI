using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoAPI.Migrations
{
    /// <inheritdoc />
    public partial class tst : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "symbol",
                table: "CryptoKandles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "source",
                table: "CryptoKandles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "update_date",
                table: "CryptoKandles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "circulating_supply",
                table: "Crypto_Symbols",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "domination",
                table: "Crypto_Symbols",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "max_supply",
                table: "Crypto_Symbols",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "source",
                table: "Crypto_Symbols",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "total_supply",
                table: "Crypto_Symbols",
                type: "float",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Symbol",
                table: "Crypto_Price",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Crypto_Price",
                type: "decimal(20,10)",
                precision: 20,
                scale: 10,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,10)",
                oldPrecision: 20,
                oldScale: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTime",
                table: "Crypto_Price",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateTable(
                name: "InstrumentsNews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    publishername = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    publisherhomepage_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    publisherlogo_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    publisherfavicon_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    author = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    published_utc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    article_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    image_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    amp_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstrumentsNews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockDescription",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ticker = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    market = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    locale = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    primary_exchange = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: true),
                    currency_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cik = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    composite_figi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    share_class_figi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    market_cap = table.Column<double>(type: "float", nullable: true),
                    phone_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    address1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    city = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    state = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    postal_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sic_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sic_description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ticker_root = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    homepage_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    total_employees = table.Column<int>(type: "int", nullable: true),
                    list_date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    logo_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    icon_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    share_class_shares_outstanding = table.Column<double>(type: "float", nullable: true),
                    weighted_shares_outstanding = table.Column<double>(type: "float", nullable: true),
                    round_lot = table.Column<int>(type: "int", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockDescription", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockInstruments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ticker = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    market = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    currency_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    composite_figi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    share_class_figi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    locale = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockInstruments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TickerToNews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ticker = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstrumentsNewsId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TickerToNews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TickerToNews_InstrumentsNews_InstrumentsNewsId",
                        column: x => x.InstrumentsNewsId,
                        principalTable: "InstrumentsNews",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TickerToNews_InstrumentsNewsId",
                table: "TickerToNews",
                column: "InstrumentsNewsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockDescription");

            migrationBuilder.DropTable(
                name: "StockInstruments");

            migrationBuilder.DropTable(
                name: "TickerToNews");

            migrationBuilder.DropTable(
                name: "InstrumentsNews");

            migrationBuilder.DropColumn(
                name: "update_date",
                table: "CryptoKandles");

            migrationBuilder.DropColumn(
                name: "circulating_supply",
                table: "Crypto_Symbols");

            migrationBuilder.DropColumn(
                name: "domination",
                table: "Crypto_Symbols");

            migrationBuilder.DropColumn(
                name: "max_supply",
                table: "Crypto_Symbols");

            migrationBuilder.DropColumn(
                name: "source",
                table: "Crypto_Symbols");

            migrationBuilder.DropColumn(
                name: "total_supply",
                table: "Crypto_Symbols");

            migrationBuilder.AlterColumn<string>(
                name: "symbol",
                table: "CryptoKandles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "source",
                table: "CryptoKandles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Symbol",
                table: "Crypto_Price",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Crypto_Price",
                type: "decimal(20,10)",
                precision: 20,
                scale: 10,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,10)",
                oldPrecision: 20,
                oldScale: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTime",
                table: "Crypto_Price",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
