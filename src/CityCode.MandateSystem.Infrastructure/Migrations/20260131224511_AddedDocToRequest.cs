using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityCode.MandateSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedDocToRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "MandateRequestId",
                table: "Documents",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_MandateRequestId",
                table: "Documents",
                column: "MandateRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_MandateRequests_MandateRequestId",
                table: "Documents",
                column: "MandateRequestId",
                principalTable: "MandateRequests",
                principalColumn: "MandateRequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_MandateRequests_MandateRequestId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_MandateRequestId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "MandateRequestId",
                table: "Documents");
        }
    }
}
