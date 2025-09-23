using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CityCode.MandateSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedRetry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "TakeCharge",
                table: "Mandates",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TakeCharge",
                table: "MandateRequests",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "RetryTransactions",
                columns: table => new
                {
                    RetryTransactionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MandateId = table.Column<long>(type: "bigint", nullable: false),
                    IsPosted = table.Column<bool>(type: "boolean", nullable: false),
                    ResponseCode = table.Column<string>(type: "text", nullable: true),
                    ResponseDescription = table.Column<string>(type: "text", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    NextRunDate = table.Column<DateOnly>(type: "date", nullable: false),
                    RetryCount = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    DateCreated = table.Column<DateOnly>(type: "date", nullable: false),
                    TimeCreated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_RetryTransactions", x => x.RetryTransactionId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RetryTransactions");

            migrationBuilder.DropColumn(
                name: "TakeCharge",
                table: "Mandates");

            migrationBuilder.DropColumn(
                name: "TakeCharge",
                table: "MandateRequests");
        }
    }
}
