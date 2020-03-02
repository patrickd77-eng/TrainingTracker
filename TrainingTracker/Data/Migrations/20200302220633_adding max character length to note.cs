using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainingTracker.Data.Migrations
{
    public partial class addingmaxcharacterlengthtonote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NoteContent",
                table: "Note",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NoteContent",
                table: "Note",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 250);
        }
    }
}
