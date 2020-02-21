using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainingTracker.Data.Migrations
{
    public partial class ChangeEmployeeModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "status",
                table: "Employees",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "fullName",
                table: "Employees",
                newName: "FullName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Employees",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Employees",
                newName: "fullName");
        }
    }
}
