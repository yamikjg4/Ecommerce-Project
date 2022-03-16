using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace E_Commerce.Migrations
{
    public partial class tblorder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblorder",
                columns: table => new
                {
                    orderid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ad_id1 = table.Column<int>(type: "int", nullable: true),
                    productidproduct_id = table.Column<int>(type: "int", nullable: true),
                    userid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    totalpay = table.Column<int>(type: "int", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblorder", x => x.orderid);
                    table.ForeignKey(
                        name: "FK_tblorder_tblAddresses_ad_id1",
                        column: x => x.ad_id1,
                        principalTable: "tblAddresses",
                        principalColumn: "ad_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblorder_tblproduct_productidproduct_id",
                        column: x => x.productidproduct_id,
                        principalTable: "tblproduct",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblorder_ad_id1",
                table: "tblorder",
                column: "ad_id1");

            migrationBuilder.CreateIndex(
                name: "IX_tblorder_productidproduct_id",
                table: "tblorder",
                column: "productidproduct_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblorder");
        }
    }
}
