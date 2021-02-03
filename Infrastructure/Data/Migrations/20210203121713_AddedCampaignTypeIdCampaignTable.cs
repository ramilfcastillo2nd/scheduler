using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class AddedCampaignTypeIdCampaignTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CampaignType",
                table: "Campaigns");

            migrationBuilder.AddColumn<int>(
                name: "CampaignTypeId",
                table: "Campaigns",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CampaignTypeId",
                table: "Campaigns");

            migrationBuilder.AddColumn<int>(
                name: "CampaignType",
                table: "Campaigns",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
