using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CityCode.MandateSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDocumentDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_Mandates_MandateReference",
                table: "Mandates",
                column: "MandateReference");

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    DocumentId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MandateReference = table.Column<string>(type: "character varying(100)", nullable: true),
                    DocumentName = table.Column<string>(type: "text", nullable: false),
                    ContentType = table.Column<string>(type: "text", nullable: false),
                    FileData = table.Column<byte[]>(type: "bytea", nullable: false),
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
                    table.PrimaryKey("PK_Documents", x => x.DocumentId);
                    table.ForeignKey(
                        name: "FK_Documents_Mandates_MandateReference",
                        column: x => x.MandateReference,
                        principalTable: "Mandates",
                        principalColumn: "MandateReference");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_MandateReference",
                table: "Documents",
                column: "MandateReference");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Mandates_MandateReference",
                table: "Mandates");
        }
    }
}
