using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VaaradhiPay.Migrations
{
    /// <inheritdoc />
    public partial class Upi_Wallet_Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeStamp",
                table: "UPIAddresses",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "TimeStamp",
                table: "TetherWallets",
                newName: "CreatedDate");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "UPIAddresses",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "UpiUserName",
                table: "UPIAddresses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "WalletType",
                table: "TetherWallets",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "WalletName",
                table: "TetherWallets",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "WalletAddress",
                table: "TetherWallets",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpiUserName",
                table: "UPIAddresses");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "UPIAddresses",
                newName: "TimeStamp");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "TetherWallets",
                newName: "TimeStamp");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "UPIAddresses",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "WalletType",
                table: "TetherWallets",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "WalletName",
                table: "TetherWallets",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "WalletAddress",
                table: "TetherWallets",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
