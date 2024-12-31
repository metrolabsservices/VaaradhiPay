using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VaaradhiPay.Migrations
{
    /// <inheritdoc />
    public partial class addingCurrencyTypetoAdminUpiAccounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrencyType",
                table: "AdminUpiAccounts",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrencyType",
                table: "AdminUpiAccounts");
        }
    }
}
