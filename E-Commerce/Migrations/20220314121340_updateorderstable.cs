using Microsoft.EntityFrameworkCore.Migrations;

namespace E_Commerce.Migrations
{
    public partial class updateorderstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblorder_tblAddresses_ad_id1",
                table: "tblorder");

            migrationBuilder.RenameColumn(
                name: "ad_id1",
                table: "tblorder",
                newName: "ad_id");

            migrationBuilder.RenameIndex(
                name: "IX_tblorder_ad_id1",
                table: "tblorder",
                newName: "IX_tblorder_ad_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tblorder_tblAddresses_ad_id",
                table: "tblorder",
                column: "ad_id",
                principalTable: "tblAddresses",
                principalColumn: "ad_id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblorder_tblAddresses_ad_id",
                table: "tblorder");

            migrationBuilder.RenameColumn(
                name: "ad_id",
                table: "tblorder",
                newName: "ad_id1");

            migrationBuilder.RenameIndex(
                name: "IX_tblorder_ad_id",
                table: "tblorder",
                newName: "IX_tblorder_ad_id1");

            migrationBuilder.AddForeignKey(
                name: "FK_tblorder_tblAddresses_ad_id1",
                table: "tblorder",
                column: "ad_id1",
                principalTable: "tblAddresses",
                principalColumn: "ad_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
