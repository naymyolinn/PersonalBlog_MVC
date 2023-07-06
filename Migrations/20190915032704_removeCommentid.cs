using Microsoft.EntityFrameworkCore.Migrations;

namespace personalblog.Migrations
{
    public partial class removeCommentid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Commentid",
                table: "Posts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Commentid",
                table: "Posts",
                nullable: false,
                defaultValue: 0);
        }
    }
}
