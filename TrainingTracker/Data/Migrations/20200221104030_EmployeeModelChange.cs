using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainingTracker.Data.Migrations
{
    public partial class EmployeeModelChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "overallProgress",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "fullName",
                table: "Employees",
                newName: "FullName");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Employees",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Employees",
                newName: "fullName");

            migrationBuilder.AddColumn<int>(
                name: "overallProgress",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
