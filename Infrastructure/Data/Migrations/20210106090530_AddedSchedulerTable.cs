using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class AddedSchedulerTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Schedulers",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    MiddleName = table.Column<string>(nullable: true),
                    JobTitle = table.Column<string>(nullable: true),
                    Company = table.Column<string>(nullable: true),
                    Industry = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    LinkedInUrl = table.Column<string>(nullable: true),
                    ConnectionRequest = table.Column<bool>(nullable: true),
                    Date = table.Column<DateTime>(nullable: true),
                    RequestAccepted = table.Column<bool>(nullable: true),
                    IsMessage1 = table.Column<bool>(nullable: true),
                    DateMessage1 = table.Column<DateTime>(nullable: true),
                    IsMessage2 = table.Column<bool>(nullable: true),
                    DateMessage2 = table.Column<DateTime>(nullable: true),
                    IsMessage3 = table.Column<bool>(nullable: true),
                    DateMessage3 = table.Column<DateTime>(nullable: true),
                    IsMessage4 = table.Column<bool>(nullable: true),
                    DateMessage4 = table.Column<DateTime>(nullable: true),
                    IsMessage5 = table.Column<bool>(nullable: true),
                    DateMessage5 = table.Column<DateTime>(nullable: true),
                    Neutral = table.Column<string>(nullable: true),
                    Negative = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedulers", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Schedulers");
        }
    }
}
