using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Migrations
{
    public partial class BodyNvarchar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Body",
                table: "Articles",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Body",
                table: "Articles",
                type: "ntext",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
