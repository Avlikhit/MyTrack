using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyTrack.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectPaySettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "FederalTaxPercent",
                table: "Projects",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "HourlyRate",
                table: "Projects",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MedicareTaxPercent",
                table: "Projects",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SocialSecurityTaxPercent",
                table: "Projects",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "StateTaxPercent",
                table: "Projects",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FederalTaxPercent",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "HourlyRate",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "MedicareTaxPercent",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "SocialSecurityTaxPercent",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "StateTaxPercent",
                table: "Projects");
        }
    }
}
