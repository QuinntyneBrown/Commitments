using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DashboardService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DashboardAddProfileId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProfileId",
                schema: "Dashboard",
                table: "Dashboards",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileId",
                schema: "Dashboard",
                table: "Dashboards");
        }
    }
}
