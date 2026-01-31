using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityCode.MandateSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RelaxConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Mandates_MandateReference",
                table: "Documents");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Mandates_MandateReference",
                table: "Mandates");

            migrationBuilder.DropIndex(
                name: "IX_Documents_MandateReference",
                table: "Documents");

            migrationBuilder.AlterColumn<string>(
                name: "MandateReference",
                table: "Documents",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MandateReference",
                table: "Documents",
                type: "character varying(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Mandates_MandateReference",
                table: "Mandates",
                column: "MandateReference");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_MandateReference",
                table: "Documents",
                column: "MandateReference");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Mandates_MandateReference",
                table: "Documents",
                column: "MandateReference",
                principalTable: "Mandates",
                principalColumn: "MandateReference");
        }
    }
}
