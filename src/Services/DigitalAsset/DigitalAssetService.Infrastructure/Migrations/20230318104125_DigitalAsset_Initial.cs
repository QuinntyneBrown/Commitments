using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalAssetService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DigitalAssetInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "DigitalAssets");

            migrationBuilder.CreateTable(
                name: "DigitalAssets",
                schema: "DigitalAssets",
                columns: table => new
                {
                    DigitalAssetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bytes = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DigitalAssets", x => x.DigitalAssetId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DigitalAssets",
                schema: "DigitalAssets");
        }
    }
}