using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityCode.MandateSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovedPropInitiatedBy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InitiatedBy",
                table: "MandateRequests");

            migrationBuilder.DropColumn(
                name: "InitiatedById",
                table: "MandateRequests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InitiatedBy",
                table: "MandateRequests",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "InitiatedById",
                table: "MandateRequests",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
