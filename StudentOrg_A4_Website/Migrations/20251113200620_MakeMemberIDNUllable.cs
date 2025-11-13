using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentOrg_A4_Website.Migrations
{
    /// <inheritdoc />
    public partial class MakeMemberIDNUllable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_members_MemberId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "MemberId",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_members_MemberId",
                table: "AspNetUsers",
                column: "MemberId",
                principalTable: "members",
                principalColumn: "member_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_members_MemberId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "MemberId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_members_MemberId",
                table: "AspNetUsers",
                column: "MemberId",
                principalTable: "members",
                principalColumn: "member_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
