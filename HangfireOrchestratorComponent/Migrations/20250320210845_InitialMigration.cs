using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HangfireOrchestratorComponent.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Workflows",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Task1JobId = table.Column<string>(type: "text", nullable: false),
                    Task2JobId = table.Column<string>(type: "text", nullable: false),
                    Task3JobId = table.Column<string>(type: "text", nullable: false),
                    Task4JobId = table.Column<string>(type: "text", nullable: false),
                    Task5JobId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workflows", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Workflows");
        }
    }
}
