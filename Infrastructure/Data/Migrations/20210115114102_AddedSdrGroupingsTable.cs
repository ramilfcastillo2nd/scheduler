using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class AddedSdrGroupingsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SdrGroupings",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ManagerId = table.Column<int>(nullable: true),
                    SdrId = table.Column<int>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SdrGroupings", x => x.id);
                    table.ForeignKey(
                        name: "FK_SdrGroupings_UserProfiles_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "UserProfiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SdrGroupings_UserProfiles_SdrId",
                        column: x => x.SdrId,
                        principalTable: "UserProfiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SdrGroupings_ManagerId",
                table: "SdrGroupings",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_SdrGroupings_SdrId",
                table: "SdrGroupings",
                column: "SdrId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SdrGroupings");
        }
    }
}
