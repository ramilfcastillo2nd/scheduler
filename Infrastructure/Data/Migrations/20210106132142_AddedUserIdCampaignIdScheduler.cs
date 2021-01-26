using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class AddedUserIdCampaignIdScheduler : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CampaignId",
                table: "Schedulers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserProfileId",
                table: "Schedulers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedulers_CampaignId",
                table: "Schedulers",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedulers_UserProfileId",
                table: "Schedulers",
                column: "UserProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedulers_Campaigns_CampaignId",
                table: "Schedulers",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedulers_UserProfiles_UserProfileId",
                table: "Schedulers",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedulers_Campaigns_CampaignId",
                table: "Schedulers");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedulers_UserProfiles_UserProfileId",
                table: "Schedulers");

            migrationBuilder.DropIndex(
                name: "IX_Schedulers_CampaignId",
                table: "Schedulers");

            migrationBuilder.DropIndex(
                name: "IX_Schedulers_UserProfileId",
                table: "Schedulers");

            migrationBuilder.DropColumn(
                name: "CampaignId",
                table: "Schedulers");

            migrationBuilder.DropColumn(
                name: "UserProfileId",
                table: "Schedulers");
        }
    }
}
