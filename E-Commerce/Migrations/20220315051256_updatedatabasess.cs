using Microsoft.EntityFrameworkCore.Migrations;

namespace E_Commerce.Migrations
{
    public partial class updatedatabasess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblorder_tblAddresses_qty",
                table: "tblorder");

            migrationBuilder.RenameColumn(
                name: "qty",
                table: "tblorder",
                newName: "qtys");

            migrationBuilder.RenameIndex(
                name: "IX_tblorder_qty",
                table: "tblorder",
                newName: "IX_tblorder_qtys");

            migrationBuilder.AddForeignKey(
                name: "FK_tblorder_tblAddresses_qtys",
                table: "tblorder",
                column: "qtys",
                principalTable: "tblAddresses",
                principalColumn: "ad_id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblorder_tblAddresses_qtys",
                table: "tblorder");

            migrationBuilder.RenameColumn(
                name: "qtys",
                table: "tblorder",
                newName: "qty");

            migrationBuilder.RenameIndex(
                name: "IX_tblorder_qtys",
                table: "tblorder",
                newName: "IX_tblorder_qty");

            migrationBuilder.AddForeignKey(
                name: "FK_tblorder_tblAddresses_qty",
                table: "tblorder",
                column: "qty",
                principalTable: "tblAddresses",
                principalColumn: "ad_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
