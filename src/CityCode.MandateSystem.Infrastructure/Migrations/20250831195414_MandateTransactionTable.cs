using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CityCode.MandateSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MandateTransactionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MandateTransactions",
                columns: table => new
                {
                    MandateTransactionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MandateScheduleId = table.Column<long>(type: "bigint", nullable: false),
                    MandateId = table.Column<long>(type: "bigint", nullable: false),
                    TransactionReference = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    TransactionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    TransactionTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TransactionId = table.Column<string>(type: "text", nullable: true),
                    TransactionStatus = table.Column<string>(type: "text", nullable: false),
                    StatusMessage = table.Column<string>(type: "text", nullable: true),
                    ResponseCode = table.Column<string>(type: "text", nullable: false),
                    SessionId = table.Column<string>(type: "text", nullable: false),
                    ChannelCode = table.Column<int>(type: "integer", nullable: false),
                    NameEnquiryRef = table.Column<string>(type: "text", nullable: false),
                    DestinationInstitutionCode = table.Column<string>(type: "text", nullable: false),
                    BeneficiaryAccountName = table.Column<string>(type: "text", nullable: false),
                    BeneficiaryAccountNumber = table.Column<string>(type: "text", nullable: false),
                    BeneficiaryKYCLevel = table.Column<string>(type: "text", nullable: false),
                    BeneficiaryBankVerificationNumber = table.Column<string>(type: "text", nullable: false),
                    OriginatorAccountName = table.Column<string>(type: "text", nullable: false),
                    OriginatorAccountNumber = table.Column<string>(type: "text", nullable: false),
                    OriginatorBankVerificationNumber = table.Column<string>(type: "text", nullable: false),
                    OriginatorKYCLevel = table.Column<string>(type: "text", nullable: false),
                    TransactionLocation = table.Column<string>(type: "text", nullable: false),
                    Narration = table.Column<string>(type: "text", nullable: false),
                    PaymentReference = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_MandateTransactions", x => x.MandateTransactionId);
                    table.ForeignKey(
                        name: "FK_MandateTransactions_MandateSchedules_MandateScheduleId",
                        column: x => x.MandateScheduleId,
                        principalTable: "MandateSchedules",
                        principalColumn: "MandateScheduleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MandateTransactions_MandateScheduleId",
                table: "MandateTransactions",
                column: "MandateScheduleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MandateTransactions");
        }
    }
}
