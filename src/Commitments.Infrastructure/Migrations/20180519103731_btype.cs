using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Commitments.Infrastructure.Migrations
{
    public partial class btype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Behaviours_BehaviourTypes_BehaviourTypeId",
                table: "Behaviours");

            migrationBuilder.AlterColumn<int>(
                name: "BehaviourTypeId",
                table: "Behaviours",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Behaviours_BehaviourTypes_BehaviourTypeId",
                table: "Behaviours",
                column: "BehaviourTypeId",
                principalTable: "BehaviourTypes",
                principalColumn: "BehaviourTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Behaviours_BehaviourTypes_BehaviourTypeId",
                table: "Behaviours");

            migrationBuilder.AlterColumn<int>(
                name: "BehaviourTypeId",
                table: "Behaviours",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Behaviours_BehaviourTypes_BehaviourTypeId",
                table: "Behaviours",
                column: "BehaviourTypeId",
                principalTable: "BehaviourTypes",
                principalColumn: "BehaviourTypeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
