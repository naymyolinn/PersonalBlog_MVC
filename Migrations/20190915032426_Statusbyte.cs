using Microsoft.EntityFrameworkCore.Migrations;

namespace personalblog.Migrations
{
    public partial class Statusbyte : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "Posts",
                nullable: false,
                oldClrType: typeof(short));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "Status",
                table: "Posts",
                nullable: false,
                oldClrType: typeof(byte));
        }
    }
}
