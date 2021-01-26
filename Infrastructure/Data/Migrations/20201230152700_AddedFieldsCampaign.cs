using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class AddedFieldsCampaign : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountsExecutive",
                table: "Campaigns",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientEmail",
                table: "Campaigns",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrelloEditor",
                table: "Campaigns",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountsExecutive",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "ClientEmail",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "TrelloEditor",
                table: "Campaigns");
        }
    }
}
