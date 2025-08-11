using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CityCode.MandateSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedScheduleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MandateSchedules",
                columns: table => new
                {
                    MandateScheduleId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MandateId = table.Column<long>(type: "bigint", nullable: false),
                    MandateReference = table.Column<string>(type: "text", nullable: false),
                    NibbsMandateCode = table.Column<string>(type: "text", nullable: false),
                    WorkflowStatus = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    NextRunDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PaymentFrequency = table.Column<int>(type: "integer", nullable: false),
                    IsEnded = table.Column<bool>(type: "boolean", nullable: false),
                    DateOfBankApproval = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
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
                    table.PrimaryKey("PK_MandateSchedules", x => x.MandateScheduleId);
                    table.ForeignKey(
                        name: "FK_MandateSchedules_Mandates_MandateId",
                        column: x => x.MandateId,
                        principalTable: "Mandates",
                        principalColumn: "MandateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MandateSchedules_MandateId",
                table: "MandateSchedules",
                column: "MandateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MandateSchedules");
        }
    }
}
