using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainingTracker.Data.Migrations
{
    public partial class TestingNewDatabaseStructureFour : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Progress_Employees_EmployeeId",
                table: "Progress");

            migrationBuilder.DropForeignKey(
                name: "FK_Progress_Training_TrainingId",
                table: "Progress");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employees",
                table: "Employees");

            migrationBuilder.RenameTable(
                name: "Employees",
                newName: "Employee");

            migrationBuilder.AlterColumn<int>(
                name: "TrainingId",
                table: "Progress",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "Progress",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Completed",
                table: "Progress",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employee",
                table: "Employee",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Progress_Employee_EmployeeId",
                table: "Progress",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Progress_Training_TrainingId",
                table: "Progress",
                column: "TrainingId",
                principalTable: "Training",
                principalColumn: "TrainingId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Progress_Employee_EmployeeId",
                table: "Progress");

            migrationBuilder.DropForeignKey(
                name: "FK_Progress_Training_TrainingId",
                table: "Progress");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employee",
                table: "Employee");

            migrationBuilder.RenameTable(
                name: "Employee",
                newName: "Employees");

            migrationBuilder.AlterColumn<int>(
                name: "TrainingId",
                table: "Progress",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "Progress",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "Completed",
                table: "Progress",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employees",
                table: "Employees",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Progress_Employees_EmployeeId",
                table: "Progress",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Progress_Training_TrainingId",
                table: "Progress",
                column: "TrainingId",
                principalTable: "Training",
                principalColumn: "TrainingId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
