using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIInterview.API.Migrations
{
    /// <inheritdoc />
    public partial class newTopic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_TopicModel_TopicId",
                table: "Question");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TopicModel",
                table: "TopicModel");

            migrationBuilder.RenameTable(
                name: "TopicModel",
                newName: "Topic");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Topic",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Topic",
                table: "Topic",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Topic_Name",
                table: "Topic",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Topic_TopicId",
                table: "Question",
                column: "TopicId",
                principalTable: "Topic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_Topic_TopicId",
                table: "Question");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Topic",
                table: "Topic");

            migrationBuilder.DropIndex(
                name: "IX_Topic_Name",
                table: "Topic");

            migrationBuilder.RenameTable(
                name: "Topic",
                newName: "TopicModel");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TopicModel",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TopicModel",
                table: "TopicModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_TopicModel_TopicId",
                table: "Question",
                column: "TopicId",
                principalTable: "TopicModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
