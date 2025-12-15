using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Commitments.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Commitments_Initial : Migration
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
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BehaviourTypes", x => x.BehaviourTypeId);
                });

            migrationBuilder.CreateTable(
                name: "CardLayouts",
                schema: "Commitments",
                columns: table => new
                {
                    CardLayoutId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
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
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
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
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
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
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bytes = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DigitalAssets", x => x.DigitalAssetId);
                });

            migrationBuilder.CreateTable(
                name: "FrequencyTypes",
                schema: "Commitments",
                columns: table => new
                {
                    FrequencyTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.NoteId);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                schema: "Commitments",
                columns: table => new
                {
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.ProfileId);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                schema: "Commitments",
                columns: table => new
                {
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.TagId);
                });

            migrationBuilder.CreateTable(
                name: "Behaviours",
                schema: "Commitments",
                columns: table => new
                {
                    BehaviourId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                name: "DashboardCards",
                schema: "Commitments",
                columns: table => new
                {
                    DashboardCardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DashboardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CardLayoutId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Options = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
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
                name: "ToDos",
                schema: "Commitments",
                columns: table => new
                {
                    ToDoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DueOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDos", x => x.ToDoId);
                    table.ForeignKey(
                        name: "FK_ToDos_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalSchema: "Commitments",
                        principalTable: "Profiles",
                        principalColumn: "ProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NoteTag",
                schema: "Commitments",
                columns: table => new
                {
                    NoteTagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NoteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteTag", x => x.NoteTagId);
                    table.ForeignKey(
                        name: "FK_NoteTag_Notes_NoteId",
                        column: x => x.NoteId,
                        principalSchema: "Commitments",
                        principalTable: "Notes",
                        principalColumn: "NoteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NoteTag_Tags_TagId",
                        column: x => x.TagId,
                        principalSchema: "Commitments",
                        principalTable: "Tags",
                        principalColumn: "TagId",
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
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.ForeignKey(
                        name: "FK_Activities_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalSchema: "Commitments",
                        principalTable: "Profiles",
                        principalColumn: "ProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Commitment",
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
                    table.PrimaryKey("PK_Commitment", x => x.CommitmentId);
                    table.ForeignKey(
                        name: "FK_Commitment_Behaviours_BehaviourId",
                        column: x => x.BehaviourId,
                        principalSchema: "Commitments",
                        principalTable: "Behaviours",
                        principalColumn: "BehaviourId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommitmentFrequency",
                schema: "Commitments",
                columns: table => new
                {
                    CommitmentFrequencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommitmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FrequencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommitmentFrequency", x => x.CommitmentFrequencyId);
                    table.ForeignKey(
                        name: "FK_CommitmentFrequency_Commitment_CommitmentId",
                        column: x => x.CommitmentId,
                        principalSchema: "Commitments",
                        principalTable: "Commitment",
                        principalColumn: "CommitmentId");
                    table.ForeignKey(
                        name: "FK_CommitmentFrequency_Frequencies_FrequencyId",
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
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommitmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommitmentPreCondition", x => x.CommitmentPreConditionId);
                    table.ForeignKey(
                        name: "FK_CommitmentPreCondition_Commitment_CommitmentId",
                        column: x => x.CommitmentId,
                        principalSchema: "Commitments",
                        principalTable: "Commitment",
                        principalColumn: "CommitmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_BehaviourId",
                schema: "Commitments",
                table: "Activities",
                column: "BehaviourId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ProfileId",
                schema: "Commitments",
                table: "Activities",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Behaviours_BehaviourTypeId",
                schema: "Commitments",
                table: "Behaviours",
                column: "BehaviourTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Commitment_BehaviourId",
                schema: "Commitments",
                table: "Commitment",
                column: "BehaviourId");

            migrationBuilder.CreateIndex(
                name: "IX_CommitmentFrequency_CommitmentId",
                schema: "Commitments",
                table: "CommitmentFrequency",
                column: "CommitmentId");

            migrationBuilder.CreateIndex(
                name: "IX_CommitmentFrequency_FrequencyId",
                schema: "Commitments",
                table: "CommitmentFrequency",
                column: "FrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_CommitmentPreCondition_CommitmentId",
                schema: "Commitments",
                table: "CommitmentPreCondition",
                column: "CommitmentId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Frequencies_FrequencyTypeId",
                schema: "Commitments",
                table: "Frequencies",
                column: "FrequencyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteTag_NoteId",
                schema: "Commitments",
                table: "NoteTag",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteTag_TagId",
                schema: "Commitments",
                table: "NoteTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_ToDos_ProfileId",
                schema: "Commitments",
                table: "ToDos",
                column: "ProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activities",
                schema: "Commitments");

            migrationBuilder.DropTable(
                name: "CommitmentFrequency",
                schema: "Commitments");

            migrationBuilder.DropTable(
                name: "CommitmentPreCondition",
                schema: "Commitments");

            migrationBuilder.DropTable(
                name: "DashboardCards",
                schema: "Commitments");

            migrationBuilder.DropTable(
                name: "DigitalAssets",
                schema: "Commitments");

            migrationBuilder.DropTable(
                name: "NoteTag",
                schema: "Commitments");

            migrationBuilder.DropTable(
                name: "ToDos",
                schema: "Commitments");

            migrationBuilder.DropTable(
                name: "Frequencies",
                schema: "Commitments");

            migrationBuilder.DropTable(
                name: "Commitment",
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

            migrationBuilder.DropTable(
                name: "Notes",
                schema: "Commitments");

            migrationBuilder.DropTable(
                name: "Tags",
                schema: "Commitments");

            migrationBuilder.DropTable(
                name: "Profiles",
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