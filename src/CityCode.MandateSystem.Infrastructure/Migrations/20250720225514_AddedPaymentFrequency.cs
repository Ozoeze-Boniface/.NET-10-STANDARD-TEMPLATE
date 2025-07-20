using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityCode.MandateSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedPaymentFrequency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentFrequency",
                table: "Mandates",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PaymentFrequency",
                table: "MandateRequests",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentFrequency",
                table: "Mandates");

            migrationBuilder.DropColumn(
                name: "PaymentFrequency",
                table: "MandateRequests");
        }
    }
}
