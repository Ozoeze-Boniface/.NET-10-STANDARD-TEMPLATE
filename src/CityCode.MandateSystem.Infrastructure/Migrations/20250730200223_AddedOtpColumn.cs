using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityCode.MandateSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedOtpColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Otp",
                table: "AppUsers",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Otp",
                table: "AppUsers");
        }
    }
}
