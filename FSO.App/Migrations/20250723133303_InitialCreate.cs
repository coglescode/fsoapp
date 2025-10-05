using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSO.App.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "fso");

            migrationBuilder.CreateTable(
                name: "Members",
                schema: "fso",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberId", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SerialNumbers",
                schema: "fso",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SerialNumberId", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Members",
                schema: "fso");

            migrationBuilder.DropTable(
                name: "SerialNumbers",
                schema: "fso");
        }
    }
}
