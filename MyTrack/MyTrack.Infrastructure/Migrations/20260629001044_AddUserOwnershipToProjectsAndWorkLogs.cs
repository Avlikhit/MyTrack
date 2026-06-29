using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyTrack.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserOwnershipToProjectsAndWorkLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkLogs_Projects_ProjectId",
                table: "WorkLogs");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "WorkLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WorkLogs_UserId",
                table: "WorkLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_UserId",
                table: "Projects",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_UserId",
                table: "Projects",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkLogs_Projects_ProjectId",
                table: "WorkLogs",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkLogs_Users_UserId",
                table: "WorkLogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Users_UserId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkLogs_Projects_ProjectId",
                table: "WorkLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkLogs_Users_UserId",
                table: "WorkLogs");

            migrationBuilder.DropIndex(
                name: "IX_WorkLogs_UserId",
                table: "WorkLogs");

            migrationBuilder.DropIndex(
                name: "IX_Projects_UserId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WorkLogs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Projects");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkLogs_Projects_ProjectId",
                table: "WorkLogs",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
