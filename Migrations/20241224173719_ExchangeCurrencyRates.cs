using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VaaradhiPay.Migrations
{
    /// <inheritdoc />
    public partial class ExchangeCurrencyRates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExchangeCurrencyRates",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    Buy = table.Column<decimal>(type: "numeric", nullable: false),
                    Sell = table.Column<decimal>(type: "numeric", nullable: false),
                    Tax = table.Column<int>(type: "integer", nullable: false),
                    percentage = table.Column<int>(type: "integer", nullable: false),
                    LiveINR = table.Column<decimal>(type: "numeric", nullable: false),
                    ChangedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeCurrencyRates", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExchangeCurrencyRates");
        }
    }
}
