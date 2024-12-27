using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VaaradhiPay.Migrations
{
    /// <inheritdoc />
    public partial class exchangeRateHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExchangeRateHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "text", nullable: false),
                    ModifiedID = table.Column<string>(type: "text", nullable: false),
                    BuyRateNafa = table.Column<decimal>(type: "numeric", nullable: false),
                    SellRateNafa = table.Column<decimal>(type: "numeric", nullable: false),
                    BuyRateUsdt = table.Column<decimal>(type: "numeric", nullable: false),
                    SellRateUsdt = table.Column<decimal>(type: "numeric", nullable: false),
                    TaxAmountNafa = table.Column<decimal>(type: "numeric", nullable: true),
                    TaxAmountUsdt = table.Column<decimal>(type: "numeric", nullable: true),
                    ExchangePercentageNafa = table.Column<decimal>(type: "numeric", nullable: true),
                    ExchangePercentageUsdt = table.Column<decimal>(type: "numeric", nullable: true),
                    CurrentInrRate = table.Column<decimal>(type: "numeric", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeRateHistories", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExchangeRateHistories");
        }
    }
}
