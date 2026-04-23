using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FsoMVC.Migrations
{
    /// <inheritdoc />
    public partial class IsAllDayColumnToEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAllDay",
                schema: "fso",
                table: "Events",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAllDay",
                schema: "fso",
                table: "Events");
        }
    }
}
