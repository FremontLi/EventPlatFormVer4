using Microsoft.EntityFrameworkCore.Migrations;

namespace EventPlatFormVer4.Migrations
{
    public partial class migrations6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Sponsors_SponsorId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Participatants_Events_EventId",
                table: "Participatants");

            migrationBuilder.DropIndex(
                name: "IX_Participatants_EventId",
                table: "Participatants");

            migrationBuilder.DropIndex(
                name: "IX_Events_SponsorId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Participatants");

            migrationBuilder.DropColumn(
                name: "SponsorId",
                table: "Events");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "EventId",
                table: "Participatants",
                type: "int unsigned",
                nullable: true);

            migrationBuilder.AddColumn<uint>(
                name: "SponsorId",
                table: "Events",
                type: "int unsigned",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Participatants_EventId",
                table: "Participatants",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_SponsorId",
                table: "Events",
                column: "SponsorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Sponsors_SponsorId",
                table: "Events",
                column: "SponsorId",
                principalTable: "Sponsors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Participatants_Events_EventId",
                table: "Participatants",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
