using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dashboard.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                name: "Dashboards",
                schema: "Dashboard",
                columns: table => new
                {
                    DashboardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dashboards", x => x.DashboardId);
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
        }
    }
}
