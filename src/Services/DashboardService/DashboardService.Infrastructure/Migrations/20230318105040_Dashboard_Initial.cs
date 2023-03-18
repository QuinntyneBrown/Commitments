using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DashboardService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DashboardInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Dashboard");

            migrationBuilder.CreateTable(
                name: "CardLayouts",
                schema: "Dashboard",
                columns: table => new
                {
                    CardLayoutId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardLayouts", x => x.CardLayoutId);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                schema: "Dashboard",
                columns: table => new
                {
                    CardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.CardId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "Dashboard",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Dashboards",
                schema: "Dashboard",
                columns: table => new
                {
                    DashboardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dashboards", x => x.DashboardId);
                    table.ForeignKey(
                        name: "FK_Dashboards_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Dashboard",
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "DashboardCards",
                schema: "Dashboard",
                columns: table => new
                {
                    DashboardCardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DashboardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardLayoutId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Options = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DashboardCards", x => x.DashboardCardId);
                    table.ForeignKey(
                        name: "FK_DashboardCards_CardLayouts_CardLayoutId",
                        column: x => x.CardLayoutId,
                        principalSchema: "Dashboard",
                        principalTable: "CardLayouts",
                        principalColumn: "CardLayoutId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DashboardCards_Cards_CardId",
                        column: x => x.CardId,
                        principalSchema: "Dashboard",
                        principalTable: "Cards",
                        principalColumn: "CardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DashboardCards_Dashboards_DashboardId",
                        column: x => x.DashboardId,
                        principalSchema: "Dashboard",
                        principalTable: "Dashboards",
                        principalColumn: "DashboardId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DashboardCards_CardId",
                schema: "Dashboard",
                table: "DashboardCards",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_DashboardCards_CardLayoutId",
                schema: "Dashboard",
                table: "DashboardCards",
                column: "CardLayoutId");

            migrationBuilder.CreateIndex(
                name: "IX_DashboardCards_DashboardId",
                schema: "Dashboard",
                table: "DashboardCards",
                column: "DashboardId");

            migrationBuilder.CreateIndex(
                name: "IX_Dashboards_UserId",
                schema: "Dashboard",
                table: "Dashboards",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DashboardCards",
                schema: "Dashboard");

            migrationBuilder.DropTable(
                name: "CardLayouts",
                schema: "Dashboard");

            migrationBuilder.DropTable(
                name: "Cards",
                schema: "Dashboard");

            migrationBuilder.DropTable(
                name: "Dashboards",
                schema: "Dashboard");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "Dashboard");
        }
    }
}
