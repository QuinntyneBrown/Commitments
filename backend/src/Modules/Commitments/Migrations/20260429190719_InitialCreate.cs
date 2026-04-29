using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Commitments.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Commitments");

            migrationBuilder.CreateTable(
                name: "BehaviourTypes",
                schema: "Commitments",
                columns: table => new
                {
                    BehaviourTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BehaviourTypes", x => x.BehaviourTypeId);
                });

            migrationBuilder.CreateTable(
                name: "FrequencyTypes",
                schema: "Commitments",
                columns: table => new
                {
                    FrequencyTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrequencyTypes", x => x.FrequencyTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                schema: "Commitments",
                columns: table => new
                {
                    NoteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.NoteId);
                });

            migrationBuilder.CreateTable(
                name: "NoteTags",
                schema: "Commitments",
                columns: table => new
                {
                    NoteTagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NoteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteTags", x => x.NoteTagId);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                schema: "Commitments",
                columns: table => new
                {
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.TagId);
                });

            migrationBuilder.CreateTable(
                name: "Todos",
                schema: "Commitments",
                columns: table => new
                {
                    ToDoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DueOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Todos", x => x.ToDoId);
                });

            migrationBuilder.CreateTable(
                name: "Behaviours",
                schema: "Commitments",
                columns: table => new
                {
                    BehaviourId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BehaviourTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Behaviours", x => x.BehaviourId);
                    table.ForeignKey(
                        name: "FK_Behaviours_BehaviourTypes_BehaviourTypeId",
                        column: x => x.BehaviourTypeId,
                        principalSchema: "Commitments",
                        principalTable: "BehaviourTypes",
                        principalColumn: "BehaviourTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Frequencies",
                schema: "Commitments",
                columns: table => new
                {
                    FrequencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FrequencyTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDesirable = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Frequency = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Frequencies", x => x.FrequencyId);
                    table.ForeignKey(
                        name: "FK_Frequencies_FrequencyTypes_FrequencyTypeId",
                        column: x => x.FrequencyTypeId,
                        principalSchema: "Commitments",
                        principalTable: "FrequencyTypes",
                        principalColumn: "FrequencyTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                schema: "Commitments",
                columns: table => new
                {
                    ActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BehaviourId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PerformedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.ActivityId);
                    table.ForeignKey(
                        name: "FK_Activities_Behaviours_BehaviourId",
                        column: x => x.BehaviourId,
                        principalSchema: "Commitments",
                        principalTable: "Behaviours",
                        principalColumn: "BehaviourId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Commitments",
                schema: "Commitments",
                columns: table => new
                {
                    CommitmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BehaviourId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commitments", x => x.CommitmentId);
                    table.ForeignKey(
                        name: "FK_Commitments_Behaviours_BehaviourId",
                        column: x => x.BehaviourId,
                        principalSchema: "Commitments",
                        principalTable: "Behaviours",
                        principalColumn: "BehaviourId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommitmentFrequencies",
                schema: "Commitments",
                columns: table => new
                {
                    CommitmentFrequencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommitmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FrequencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommitmentFrequencies", x => x.CommitmentFrequencyId);
                    table.ForeignKey(
                        name: "FK_CommitmentFrequencies_Commitments_CommitmentId",
                        column: x => x.CommitmentId,
                        principalSchema: "Commitments",
                        principalTable: "Commitments",
                        principalColumn: "CommitmentId");
                    table.ForeignKey(
                        name: "FK_CommitmentFrequencies_Frequencies_FrequencyId",
                        column: x => x.FrequencyId,
                        principalSchema: "Commitments",
                        principalTable: "Frequencies",
                        principalColumn: "FrequencyId");
                });

            migrationBuilder.CreateTable(
                name: "CommitmentPreCondition",
                schema: "Commitments",
                columns: table => new
                {
                    CommitmentPreConditionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommitmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommitmentPreCondition", x => x.CommitmentPreConditionId);
                    table.ForeignKey(
                        name: "FK_CommitmentPreCondition_Commitments_CommitmentId",
                        column: x => x.CommitmentId,
                        principalSchema: "Commitments",
                        principalTable: "Commitments",
                        principalColumn: "CommitmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_BehaviourId",
                schema: "Commitments",
                table: "Activities",
                column: "BehaviourId");

            migrationBuilder.CreateIndex(
                name: "IX_Behaviours_BehaviourTypeId",
                schema: "Commitments",
                table: "Behaviours",
                column: "BehaviourTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CommitmentFrequencies_CommitmentId",
                schema: "Commitments",
                table: "CommitmentFrequencies",
                column: "CommitmentId");

            migrationBuilder.CreateIndex(
                name: "IX_CommitmentFrequencies_FrequencyId",
                schema: "Commitments",
                table: "CommitmentFrequencies",
                column: "FrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_CommitmentPreCondition_CommitmentId",
                schema: "Commitments",
                table: "CommitmentPreCondition",
                column: "CommitmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Commitments_BehaviourId",
                schema: "Commitments",
                table: "Commitments",
                column: "BehaviourId");

            migrationBuilder.CreateIndex(
                name: "IX_Frequencies_FrequencyTypeId",
                schema: "Commitments",
                table: "Frequencies",
                column: "FrequencyTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activities",
                schema: "Commitments");

            migrationBuilder.DropTable(
                name: "CommitmentFrequencies",
                schema: "Commitments");

            migrationBuilder.DropTable(
                name: "CommitmentPreCondition",
                schema: "Commitments");

            migrationBuilder.DropTable(
                name: "Notes",
                schema: "Commitments");

            migrationBuilder.DropTable(
                name: "NoteTags",
                schema: "Commitments");

            migrationBuilder.DropTable(
                name: "Tags",
                schema: "Commitments");

            migrationBuilder.DropTable(
                name: "Todos",
                schema: "Commitments");

            migrationBuilder.DropTable(
                name: "Frequencies",
                schema: "Commitments");

            migrationBuilder.DropTable(
                name: "Commitments",
                schema: "Commitments");

            migrationBuilder.DropTable(
                name: "FrequencyTypes",
                schema: "Commitments");

            migrationBuilder.DropTable(
                name: "Behaviours",
                schema: "Commitments");

            migrationBuilder.DropTable(
                name: "BehaviourTypes",
                schema: "Commitments");
        }
    }
}
