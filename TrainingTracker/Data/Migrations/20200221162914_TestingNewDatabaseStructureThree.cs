using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainingTracker.Data.Migrations
{
    public partial class TestingNewDatabaseStructureThree : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Employees");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
