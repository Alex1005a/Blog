using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Migrations
{
    public partial class ArticleTitleAndBody : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Body",
                table: "Articles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Articles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Body",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Articles");
        }
    }
}
