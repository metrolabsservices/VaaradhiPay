using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VaaradhiPay.Migrations
{
    /// <inheritdoc />
    public partial class UserNewEntity_KycstatusAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KYCstatus",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KYCstatus",
                table: "AspNetUsers");
        }
    }
}
