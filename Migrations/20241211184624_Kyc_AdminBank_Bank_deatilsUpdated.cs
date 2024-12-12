using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VaaradhiPay.Migrations
{
    /// <inheritdoc />
    public partial class Kyc_AdminBank_Bank_deatilsUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeStamp",
                table: "KYCDetails",
                newName: "CreatedDateTime");

            migrationBuilder.RenameColumn(
                name: "TimeStamp",
                table: "BankAccounts",
                newName: "UpdatedDateTime");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "KYCDetails",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "ProofFilePath",
                table: "BankAccounts",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IFSCCode",
                table: "BankAccounts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(11)",
                oldMaxLength: 11);

            migrationBuilder.AlterColumn<string>(
                name: "BankName",
                table: "BankAccounts",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "AccountNumber",
                table: "BankAccounts",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "AccountHolderName",
                table: "BankAccounts",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "AccountType",
                table: "BankAccounts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BranchCode",
                table: "BankAccounts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "BankAccounts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CurrencyType",
                table: "BankAccounts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "BankAccounts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "BankAccounts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "AdminBankAccounts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "KYCDetails");

            migrationBuilder.DropColumn(
                name: "AccountType",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "BranchCode",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "CurrencyType",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "AdminBankAccounts");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTime",
                table: "KYCDetails",
                newName: "TimeStamp");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "BankAccounts",
                newName: "TimeStamp");

            migrationBuilder.AlterColumn<string>(
                name: "ProofFilePath",
                table: "BankAccounts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "IFSCCode",
                table: "BankAccounts",
                type: "character varying(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BankName",
                table: "BankAccounts",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "AccountNumber",
                table: "BankAccounts",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "AccountHolderName",
                table: "BankAccounts",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
