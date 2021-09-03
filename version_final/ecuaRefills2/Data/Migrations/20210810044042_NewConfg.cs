using Microsoft.EntityFrameworkCore.Migrations;

namespace ecuaRefills2.Data.Migrations
{
    public partial class NewConfg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ProvinciaId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProvinciaId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProvinciaId",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProvinciaId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProvinciaId",
                table: "AspNetUsers",
                column: "ProvinciaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ProvinciaId",
                table: "AspNetUsers",
                column: "ProvinciaId",
                principalTable: "Provincia",
                principalColumn: "ProvinciaId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
