using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VaaradhiPay.Migrations
{
    /// <inheritdoc />
    public partial class delteEnuminAdminUpiAccounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrencyType",
                table: "AdminUpiAccounts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrencyType",
                table: "AdminUpiAccounts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
