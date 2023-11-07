using Microsoft.EntityFrameworkCore.Migrations;

namespace EvaPharmaTask.Infrastructure.Data.Migrations
{
    public partial class MakeGradeNullableInEnrollmentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_Grade_GradeId",
                table: "Enrollment");

            migrationBuilder.AlterColumn<int>(
                name: "GradeId",
                table: "Enrollment",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_Grade_GradeId",
                table: "Enrollment",
                column: "GradeId",
                principalTable: "Grade",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_Grade_GradeId",
                table: "Enrollment");

            migrationBuilder.AlterColumn<int>(
                name: "GradeId",
                table: "Enrollment",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_Grade_GradeId",
                table: "Enrollment",
                column: "GradeId",
                principalTable: "Grade",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
