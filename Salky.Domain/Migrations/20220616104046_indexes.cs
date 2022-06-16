using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Salky.Domain.Migrations
{
    public partial class indexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_NormalizedUserName",
                table: "Users",
                column: "NormalizedUserName");

            migrationBuilder.CreateIndex(
                name: "IX_MessagesGroup_SenderId_GroupId",
                table: "MessagesGroup",
                columns: new[] { "SenderId", "GroupId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_NormalizedUserName",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_MessagesGroup_SenderId_GroupId",
                table: "MessagesGroup");
        }
    }
}
