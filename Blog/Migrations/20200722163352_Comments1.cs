using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Migrations
{
    public partial class Comments1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Articles_ArticleId1",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ArticleId1",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ArticleId1",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "ArticleId",
                table: "Comments",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ArticleId",
                table: "Comments",
                column: "ArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Articles_ArticleId",
                table: "Comments",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Articles_ArticleId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ArticleId",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "ArticleId",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "ArticleId1",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ArticleId1",
                table: "Comments",
                column: "ArticleId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Articles_ArticleId1",
                table: "Comments",
                column: "ArticleId1",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
