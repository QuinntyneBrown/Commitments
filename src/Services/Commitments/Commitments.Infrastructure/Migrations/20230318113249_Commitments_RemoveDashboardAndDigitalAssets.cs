using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Commitments.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Commitments_RemoveDashboardAndDigitalAssets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DashboardCards",
                schema: "Commitments");

            migrationBuilder.DropTable(
                name: "DigitalAssets",
                schema: "Commitments");

            migrationBuilder.DropTable(
                name: "CardLayouts",
                schema: "Commitments");

            migrationBuilder.DropTable(
                name: "Cards",
                schema: "Commitments");

            migrationBuilder.DropTable(
                name: "Dashboards",
                schema: "Commitments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CardLayouts",
                schema: "Commitments",
                columns: table => new
                {
                    CardLayoutId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardLayouts", x => x.CardLayoutId);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                schema: "Commitments",
                columns: table => new
                {
                    CardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.CardId);
                });

            migrationBuilder.CreateTable(
                name: "Dashboards",
                schema: "Commitments",
                columns: table => new
                {
                    DashboardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dashboards", x => x.DashboardId);
                });

            migrationBuilder.CreateTable(
                name: "DigitalAssets",
                schema: "Commitments",
                columns: table => new
                {
                    DigitalAssetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    Bytes = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DigitalAssets", x => x.DigitalAssetId);
                });

            migrationBuilder.CreateTable(
                name: "DashboardCards",
                schema: "Commitments",
                columns: table => new
                {
                    DashboardCardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CardLayoutId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DashboardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Options = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DashboardCards", x => x.DashboardCardId);
                    table.ForeignKey(
                        name: "FK_DashboardCards_CardLayouts_CardLayoutId",
                        column: x => x.CardLayoutId,
                        principalSchema: "Commitments",
                        principalTable: "CardLayouts",
                        principalColumn: "CardLayoutId");
                    table.ForeignKey(
                        name: "FK_DashboardCards_Cards_CardId",
                        column: x => x.CardId,
                        principalSchema: "Commitments",
                        principalTable: "Cards",
                        principalColumn: "CardId");
                    table.ForeignKey(
                        name: "FK_DashboardCards_Dashboards_DashboardId",
                        column: x => x.DashboardId,
                        principalSchema: "Commitments",
                        principalTable: "Dashboards",
                        principalColumn: "DashboardId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DashboardCards_CardId",
                schema: "Commitments",
                table: "DashboardCards",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_DashboardCards_CardLayoutId",
                schema: "Commitments",
                table: "DashboardCards",
                column: "CardLayoutId");

            migrationBuilder.CreateIndex(
                name: "IX_DashboardCards_DashboardId",
                schema: "Commitments",
                table: "DashboardCards",
                column: "DashboardId");
        }
    }
}