using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class AddedLinkDepartmentIdToDepartment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Departments_Departmentid",
                table: "UserProfiles");

            migrationBuilder.RenameColumn(
                name: "Departmentid",
                table: "UserProfiles",
                newName: "DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_UserProfiles_Departmentid",
                table: "UserProfiles",
                newName: "IX_UserProfiles_DepartmentId");

            migrationBuilder.AddColumn<string>(
                name: "LinkedInName",
                table: "UserProfiles",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Departments_DepartmentId",
                table: "UserProfiles",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Departments_DepartmentId",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "LinkedInName",
                table: "UserProfiles");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "UserProfiles",
                newName: "Departmentid");

            migrationBuilder.RenameIndex(
                name: "IX_UserProfiles_DepartmentId",
                table: "UserProfiles",
                newName: "IX_UserProfiles_Departmentid");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Departments_Departmentid",
                table: "UserProfiles",
                column: "Departmentid",
                principalTable: "Departments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
