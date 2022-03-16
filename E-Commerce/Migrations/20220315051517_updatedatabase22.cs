using Microsoft.EntityFrameworkCore.Migrations;

namespace E_Commerce.Migrations
{
    public partial class updatedatabase22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblorder_tblAddresses_qtys",
                table: "tblorder");

            migrationBuilder.DropIndex(
                name: "IX_tblorder_qtys",
                table: "tblorder");

            migrationBuilder.CreateIndex(
                name: "IX_tblorder_ad_id",
                table: "tblorder",
                column: "ad_id");

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

            migrationBuilder.DropIndex(
                name: "IX_tblorder_ad_id",
                table: "tblorder");

            migrationBuilder.CreateIndex(
                name: "IX_tblorder_qtys",
                table: "tblorder",
                column: "qtys");

            migrationBuilder.AddForeignKey(
                name: "FK_tblorder_tblAddresses_qtys",
                table: "tblorder",
                column: "qtys",
                principalTable: "tblAddresses",
                principalColumn: "ad_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
