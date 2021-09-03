using Microsoft.EntityFrameworkCore.Migrations;

namespace ecuaRefills2.Data.Migrations
{
    public partial class CambiosRegistroUsuario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ciudad",
                table: "AspNetUsers",
                newName: "NombreCompleto");

            migrationBuilder.RenameColumn(
                name: "Apellido",
                table: "AspNetUsers",
                newName: "Dirección");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NombreCompleto",
                table: "AspNetUsers",
                newName: "Ciudad");

            migrationBuilder.RenameColumn(
                name: "Dirección",
                table: "AspNetUsers",
                newName: "Apellido");
        }
    }
}
