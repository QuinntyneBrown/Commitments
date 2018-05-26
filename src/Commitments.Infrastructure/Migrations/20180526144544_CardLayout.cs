using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Commitments.Infrastructure.Migrations
{
    public partial class CardLayout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CardLayoutId",
                table: "DashboardCards",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CardLayouts",
                columns: table => new
                {
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    LastModifiedOn = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CardLayoutId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardLayouts", x => x.CardLayoutId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DashboardCards_CardLayoutId",
                table: "DashboardCards",
                column: "CardLayoutId");

            migrationBuilder.AddForeignKey(
                name: "FK_DashboardCards_CardLayouts_CardLayoutId",
                table: "DashboardCards",
                column: "CardLayoutId",
                principalTable: "CardLayouts",
                principalColumn: "CardLayoutId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DashboardCards_CardLayouts_CardLayoutId",
                table: "DashboardCards");

            migrationBuilder.DropTable(
                name: "CardLayouts");

            migrationBuilder.DropIndex(
                name: "IX_DashboardCards_CardLayoutId",
                table: "DashboardCards");

            migrationBuilder.DropColumn(
                name: "CardLayoutId",
                table: "DashboardCards");
        }
    }
}
