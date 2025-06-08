using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forum.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedmisleadingindicator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMisleading",
                table: "ForumPosts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ForumComments",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_ForumComments_UserId",
                table: "ForumComments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumComments_AspNetUsers_UserId",
                table: "ForumComments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumComments_AspNetUsers_UserId",
                table: "ForumComments");

            migrationBuilder.DropIndex(
                name: "IX_ForumComments_UserId",
                table: "ForumComments");

            migrationBuilder.DropColumn(
                name: "IsMisleading",
                table: "ForumPosts");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ForumComments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
