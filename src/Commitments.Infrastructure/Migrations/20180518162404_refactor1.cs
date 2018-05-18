using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Commitments.Infrastructure.Migrations
{
    public partial class refactor1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommitmentFailFrequencyId",
                table: "Commitment");

            migrationBuilder.DropColumn(
                name: "CommitmentFrequencyId",
                table: "Commitment");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Commitment");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommitmentFailFrequencyId",
                table: "Commitment",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CommitmentFrequencyId",
                table: "Commitment",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Commitment",
                nullable: true);
        }
    }
}
