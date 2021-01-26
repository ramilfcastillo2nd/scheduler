using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class AddedDepartmentTableAndUpdatedFieldsUserProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateHired",
                table: "UserProfiles",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Departmentid",
                table: "UserProfiles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedInUrl",
                table: "UserProfiles",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_Departmentid",
                table: "UserProfiles",
                column: "Departmentid");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Departments_Departmentid",
                table: "UserProfiles",
                column: "Departmentid",
                principalTable: "Departments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Departments_Departmentid",
                table: "UserProfiles");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_Departmentid",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "DateHired",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "Departmentid",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "LinkedInUrl",
                table: "UserProfiles");
        }
    }
}
