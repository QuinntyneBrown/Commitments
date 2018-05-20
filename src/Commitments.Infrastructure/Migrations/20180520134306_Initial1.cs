using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Commitments.Infrastructure.Migrations
{
    public partial class Initial1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommitmentFrequency_Commitment_CommitmentId",
                table: "CommitmentFrequency");

            migrationBuilder.DropForeignKey(
                name: "FK_CommitmentFrequency_Frequencies_FrequencyId",
                table: "CommitmentFrequency");

            migrationBuilder.AlterColumn<int>(
                name: "FrequencyId",
                table: "CommitmentFrequency",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "CommitmentId",
                table: "CommitmentFrequency",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_CommitmentFrequency_Commitment_CommitmentId",
                table: "CommitmentFrequency",
                column: "CommitmentId",
                principalTable: "Commitment",
                principalColumn: "CommitmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CommitmentFrequency_Frequencies_FrequencyId",
                table: "CommitmentFrequency",
                column: "FrequencyId",
                principalTable: "Frequencies",
                principalColumn: "FrequencyId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommitmentFrequency_Commitment_CommitmentId",
                table: "CommitmentFrequency");

            migrationBuilder.DropForeignKey(
                name: "FK_CommitmentFrequency_Frequencies_FrequencyId",
                table: "CommitmentFrequency");

            migrationBuilder.AlterColumn<int>(
                name: "FrequencyId",
                table: "CommitmentFrequency",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CommitmentId",
                table: "CommitmentFrequency",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CommitmentFrequency_Commitment_CommitmentId",
                table: "CommitmentFrequency",
                column: "CommitmentId",
                principalTable: "Commitment",
                principalColumn: "CommitmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommitmentFrequency_Frequencies_FrequencyId",
                table: "CommitmentFrequency",
                column: "FrequencyId",
                principalTable: "Frequencies",
                principalColumn: "FrequencyId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
