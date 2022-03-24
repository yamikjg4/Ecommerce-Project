using Microsoft.EntityFrameworkCore.Migrations;

namespace E_Commerce.Migrations
{
    public partial class reds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblorder_tblAddresses_ad_id",
                table: "tblorder");

            migrationBuilder.DropColumn(
                name: "userid",
                table: "tblorder");

            migrationBuilder.AlterColumn<int>(
                name: "ad_id",
                table: "tblorder",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "tblorder",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TBLorderorderid",
                table: "tblorder",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblorder_Id",
                table: "tblorder",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_tblorder_TBLorderorderid",
                table: "tblorder",
                column: "TBLorderorderid");

            migrationBuilder.AddForeignKey(
                name: "FK_tblorder_AspNetUsers_Id",
                table: "tblorder",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tblorder_tblAddresses_ad_id",
                table: "tblorder",
                column: "ad_id",
                principalTable: "tblAddresses",
                principalColumn: "ad_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tblorder_tblorder_TBLorderorderid",
                table: "tblorder",
                column: "TBLorderorderid",
                principalTable: "tblorder",
                principalColumn: "orderid",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblorder_AspNetUsers_Id",
                table: "tblorder");

            migrationBuilder.DropForeignKey(
                name: "FK_tblorder_tblAddresses_ad_id",
                table: "tblorder");

            migrationBuilder.DropForeignKey(
                name: "FK_tblorder_tblorder_TBLorderorderid",
                table: "tblorder");

            migrationBuilder.DropIndex(
                name: "IX_tblorder_Id",
                table: "tblorder");

            migrationBuilder.DropIndex(
                name: "IX_tblorder_TBLorderorderid",
                table: "tblorder");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "tblorder");

            migrationBuilder.DropColumn(
                name: "TBLorderorderid",
                table: "tblorder");

            migrationBuilder.AlterColumn<int>(
                name: "ad_id",
                table: "tblorder",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "userid",
                table: "tblorder",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tblorder_tblAddresses_ad_id",
                table: "tblorder",
                column: "ad_id",
                principalTable: "tblAddresses",
                principalColumn: "ad_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
