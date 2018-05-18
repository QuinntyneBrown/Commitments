using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Commitments.Infrastructure.Migrations
{
    public partial class refactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commitment_CommitmentFailFrequency_CommitmentFailFrequencyId",
                table: "Commitment");

            migrationBuilder.DropForeignKey(
                name: "FK_Commitment_CommitmentFrequencies_CommitmentFrequencyId",
                table: "Commitment");

            migrationBuilder.DropForeignKey(
                name: "FK_CommitmentFrequencies_FrequencyType_FrequencyTypeId",
                table: "CommitmentFrequencies");

            migrationBuilder.DropTable(
                name: "CommitmentFailFrequency");

            migrationBuilder.DropIndex(
                name: "IX_Commitment_CommitmentFailFrequencyId",
                table: "Commitment");

            migrationBuilder.DropIndex(
                name: "IX_Commitment_CommitmentFrequencyId",
                table: "Commitment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FrequencyType",
                table: "FrequencyType");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "CommitmentFrequencies");

            migrationBuilder.RenameTable(
                name: "FrequencyType",
                newName: "FrequencyTypes");

            migrationBuilder.AlterColumn<int>(
                name: "FrequencyTypeId",
                table: "CommitmentFrequencies",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CommitmentId",
                table: "CommitmentFrequencies",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Frequency",
                table: "CommitmentFrequencies",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDesirable",
                table: "CommitmentFrequencies",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Commitment",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Commitment",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "Commitment",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "FrequencyTypes",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "FrequencyTypes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "FrequencyTypes",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_FrequencyTypes",
                table: "FrequencyTypes",
                column: "FrequencyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CommitmentFrequencies_CommitmentId",
                table: "CommitmentFrequencies",
                column: "CommitmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommitmentFrequencies_Commitment_CommitmentId",
                table: "CommitmentFrequencies",
                column: "CommitmentId",
                principalTable: "Commitment",
                principalColumn: "CommitmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommitmentFrequencies_FrequencyTypes_FrequencyTypeId",
                table: "CommitmentFrequencies",
                column: "FrequencyTypeId",
                principalTable: "FrequencyTypes",
                principalColumn: "FrequencyTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommitmentFrequencies_Commitment_CommitmentId",
                table: "CommitmentFrequencies");

            migrationBuilder.DropForeignKey(
                name: "FK_CommitmentFrequencies_FrequencyTypes_FrequencyTypeId",
                table: "CommitmentFrequencies");

            migrationBuilder.DropIndex(
                name: "IX_CommitmentFrequencies_CommitmentId",
                table: "CommitmentFrequencies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FrequencyTypes",
                table: "FrequencyTypes");

            migrationBuilder.DropColumn(
                name: "CommitmentId",
                table: "CommitmentFrequencies");

            migrationBuilder.DropColumn(
                name: "Frequency",
                table: "CommitmentFrequencies");

            migrationBuilder.DropColumn(
                name: "IsDesirable",
                table: "CommitmentFrequencies");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Commitment");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Commitment");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "Commitment");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "FrequencyTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "FrequencyTypes");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "FrequencyTypes");

            migrationBuilder.RenameTable(
                name: "FrequencyTypes",
                newName: "FrequencyType");

            migrationBuilder.AlterColumn<int>(
                name: "FrequencyTypeId",
                table: "CommitmentFrequencies",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CommitmentFrequencies",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FrequencyType",
                table: "FrequencyType",
                column: "FrequencyTypeId");

            migrationBuilder.CreateTable(
                name: "CommitmentFailFrequency",
                columns: table => new
                {
                    CommitmentFailFrequencyId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FrequencyTypeId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommitmentFailFrequency", x => x.CommitmentFailFrequencyId);
                    table.ForeignKey(
                        name: "FK_CommitmentFailFrequency_FrequencyType_FrequencyTypeId",
                        column: x => x.FrequencyTypeId,
                        principalTable: "FrequencyType",
                        principalColumn: "FrequencyTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Commitment_CommitmentFailFrequencyId",
                table: "Commitment",
                column: "CommitmentFailFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Commitment_CommitmentFrequencyId",
                table: "Commitment",
                column: "CommitmentFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_CommitmentFailFrequency_FrequencyTypeId",
                table: "CommitmentFailFrequency",
                column: "FrequencyTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Commitment_CommitmentFailFrequency_CommitmentFailFrequencyId",
                table: "Commitment",
                column: "CommitmentFailFrequencyId",
                principalTable: "CommitmentFailFrequency",
                principalColumn: "CommitmentFailFrequencyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Commitment_CommitmentFrequencies_CommitmentFrequencyId",
                table: "Commitment",
                column: "CommitmentFrequencyId",
                principalTable: "CommitmentFrequencies",
                principalColumn: "CommitmentFrequencyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommitmentFrequencies_FrequencyType_FrequencyTypeId",
                table: "CommitmentFrequencies",
                column: "FrequencyTypeId",
                principalTable: "FrequencyType",
                principalColumn: "FrequencyTypeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
