using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CityCode.MandateSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingMandateAndRequestTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "TodoLists",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "TO_DO_ITEMS",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "Permissions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Permissions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "Permissions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateApproved",
                table: "Permissions",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateCreated",
                table: "Permissions",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateDeleted",
                table: "Permissions",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Permissions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DeletedFlag",
                table: "Permissions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "HashValue",
                table: "Permissions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Permissions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Permissions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "LastModifiedDate",
                table: "Permissions",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastModifiedTime",
                table: "Permissions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RowVersion",
                table: "Permissions",
                type: "numeric(20,0)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Permissions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeApproved",
                table: "Permissions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TimeCreated",
                table: "Permissions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeDeleted",
                table: "Permissions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "AppUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AppUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "AppUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateApproved",
                table: "AppUsers",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateCreated",
                table: "AppUsers",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateDeleted",
                table: "AppUsers",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "AppUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DeletedFlag",
                table: "AppUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "HashValue",
                table: "AppUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AppUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "AppUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "LastModifiedDate",
                table: "AppUsers",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastModifiedTime",
                table: "AppUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RowVersion",
                table: "AppUsers",
                type: "numeric(20,0)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "AppUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeApproved",
                table: "AppUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TimeCreated",
                table: "AppUsers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeDeleted",
                table: "AppUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MandateRequests",
                columns: table => new
                {
                    MandateRequestId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MandateReference = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    InitiatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    InitiatedById = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    BillerId = table.Column<int>(type: "integer", nullable: false),
                    SubscriberCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProductTotalAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    TransactionAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    BankCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PayerName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PayerAddress = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PayerEmail = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PayerPhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    AccountName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PayerAccountNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PayerBvn = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    BanksAccountNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    BanksAccountName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    BanksBvn = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    DestinationInstitutionCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SourceInstitutionCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Narration = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    MandateType = table.Column<int>(type: "integer", nullable: false),
                    MandateRequestStatus = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Location = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    DateCreated = table.Column<DateOnly>(type: "date", nullable: false),
                    TimeCreated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    LastModifiedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    LastModifiedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ApprovedBy = table.Column<string>(type: "text", nullable: true),
                    DateApproved = table.Column<DateOnly>(type: "date", nullable: true),
                    TimeApproved = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    HashValue = table.Column<string>(type: "text", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DateDeleted = table.Column<DateOnly>(type: "date", nullable: true),
                    TimeDeleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RowVersion = table.Column<decimal>(type: "numeric(20,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MandateRequests", x => x.MandateRequestId);
                });

            migrationBuilder.CreateTable(
                name: "Mandates",
                columns: table => new
                {
                    MandateId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MandateReference = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NibbsMandateCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    BillerId = table.Column<int>(type: "integer", nullable: false),
                    WorkflowStatus = table.Column<int>(type: "integer", nullable: true),
                    MandateStatus = table.Column<int>(type: "integer", nullable: false),
                    ProgressStatus = table.Column<int>(type: "integer", nullable: false),
                    SubscriberCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProductTotalAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    TransactionAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    BankCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PayerName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PayerAddress = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PayerEmail = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PayerPhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    AccountName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PayerAccountNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PayerBvn = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    BanksAccountNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    BanksAccountName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    BanksBvn = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    DestinationInstitutionCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SourceInstitutionCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Narration = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    MandateType = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Location = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    DateCreated = table.Column<DateOnly>(type: "date", nullable: false),
                    TimeCreated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    LastModifiedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    LastModifiedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ApprovedBy = table.Column<string>(type: "text", nullable: true),
                    DateApproved = table.Column<DateOnly>(type: "date", nullable: true),
                    TimeApproved = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    HashValue = table.Column<string>(type: "text", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DateDeleted = table.Column<DateOnly>(type: "date", nullable: true),
                    TimeDeleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RowVersion = table.Column<decimal>(type: "numeric(20,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mandates", x => x.MandateId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MandateRequests");

            migrationBuilder.DropTable(
                name: "Mandates");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "TodoLists");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "TO_DO_ITEMS");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "DateApproved",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "DeletedFlag",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "HashValue",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "LastModifiedTime",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "TimeApproved",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "TimeCreated",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "TimeDeleted",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "DateApproved",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "DeletedFlag",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "HashValue",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "LastModifiedTime",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "TimeApproved",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "TimeCreated",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "TimeDeleted",
                table: "AppUsers");
        }
    }
}
