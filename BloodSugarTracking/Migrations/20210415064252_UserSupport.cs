using Microsoft.EntityFrameworkCore.Migrations;

namespace BloodSugarTracking.Migrations
{
    public partial class UserSupport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "BloodSugarTestResults",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BloodSugarTestResults_UserId",
                table: "BloodSugarTestResults",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BloodSugarTestResults_Users_UserId",
                table: "BloodSugarTestResults",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BloodSugarTestResults_Users_UserId",
                table: "BloodSugarTestResults");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_BloodSugarTestResults_UserId",
                table: "BloodSugarTestResults");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BloodSugarTestResults");
        }
    }
}
