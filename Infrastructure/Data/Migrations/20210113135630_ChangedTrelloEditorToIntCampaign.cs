using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class ChangedTrelloEditorToIntCampaign : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountsExecutive",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "TrelloEditor",
                table: "Campaigns");

            migrationBuilder.AddColumn<int>(
                name: "AccountsExecutiveId",
                table: "Campaigns",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrelloEditorId",
                table: "Campaigns",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_AccountsExecutiveId",
                table: "Campaigns",
                column: "AccountsExecutiveId");

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_TrelloEditorId",
                table: "Campaigns",
                column: "TrelloEditorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_UserProfiles_AccountsExecutiveId",
                table: "Campaigns",
                column: "AccountsExecutiveId",
                principalTable: "UserProfiles",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_UserProfiles_TrelloEditorId",
                table: "Campaigns",
                column: "TrelloEditorId",
                principalTable: "UserProfiles",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_UserProfiles_AccountsExecutiveId",
                table: "Campaigns");

            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_UserProfiles_TrelloEditorId",
                table: "Campaigns");

            migrationBuilder.DropIndex(
                name: "IX_Campaigns_AccountsExecutiveId",
                table: "Campaigns");

            migrationBuilder.DropIndex(
                name: "IX_Campaigns_TrelloEditorId",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "AccountsExecutiveId",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "TrelloEditorId",
                table: "Campaigns");

            migrationBuilder.AddColumn<string>(
                name: "AccountsExecutive",
                table: "Campaigns",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrelloEditor",
                table: "Campaigns",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
