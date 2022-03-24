using Microsoft.EntityFrameworkCore.Migrations;

namespace E_Commerce.Migrations
{
    public partial class red : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblorder_tblorder_TBLorderorderid",
                table: "tblorder");

            migrationBuilder.RenameColumn(
                name: "TBLorderorderid",
                table: "tblorder",
                newName: "orderid1");

            migrationBuilder.RenameIndex(
                name: "IX_tblorder_TBLorderorderid",
                table: "tblorder",
                newName: "IX_tblorder_orderid1");

            migrationBuilder.AddForeignKey(
                name: "FK_tblorder_tblorder_orderid1",
                table: "tblorder",
                column: "orderid1",
                principalTable: "tblorder",
                principalColumn: "orderid",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblorder_tblorder_orderid1",
                table: "tblorder");

            migrationBuilder.RenameColumn(
                name: "orderid1",
                table: "tblorder",
                newName: "TBLorderorderid");

            migrationBuilder.RenameIndex(
                name: "IX_tblorder_orderid1",
                table: "tblorder",
                newName: "IX_tblorder_TBLorderorderid");

            migrationBuilder.AddForeignKey(
                name: "FK_tblorder_tblorder_TBLorderorderid",
                table: "tblorder",
                column: "TBLorderorderid",
                principalTable: "tblorder",
                principalColumn: "orderid",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
