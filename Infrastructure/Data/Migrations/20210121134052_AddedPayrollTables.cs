using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class AddedPayrollTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserProfileId = table.Column<int>(nullable: false),
                    CampaignId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Points = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.id);
                    table.ForeignKey(
                        name: "FK_Attendances_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attendances_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payrolls",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserProfileId = table.Column<int>(nullable: false),
                    CampaignId = table.Column<int>(nullable: false),
                    IsCancelled = table.Column<bool>(nullable: true),
                    DaysActive = table.Column<int>(nullable: true),
                    IncentiveType = table.Column<int>(nullable: true),
                    IncentiveAmount = table.Column<decimal>(nullable: true),
                    IncentiveCount = table.Column<int>(nullable: true),
                    Wage = table.Column<decimal>(nullable: true),
                    BasePayAdjustment = table.Column<decimal>(nullable: true),
                    ApptSalesIncentive = table.Column<decimal>(nullable: true),
                    ReferralIncentive = table.Column<decimal>(nullable: true),
                    OtherIncentive = table.Column<decimal>(nullable: true),
                    RepliesInceentive = table.Column<decimal>(nullable: true),
                    Total = table.Column<decimal>(nullable: true),
                    SubTotal = table.Column<decimal>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payrolls", x => x.id);
                    table.ForeignKey(
                        name: "FK_Payrolls_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payrolls_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_CampaignId",
                table: "Attendances",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_UserProfileId",
                table: "Attendances",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Payrolls_CampaignId",
                table: "Payrolls",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_Payrolls_UserProfileId",
                table: "Payrolls",
                column: "UserProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "Payrolls");
        }
    }
}
