using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace E_Commerce.Migrations
{
    public partial class addeupdatedordertables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblorderupdated");

            migrationBuilder.CreateTable(
                name: "tblstatus",
                columns: table => new
                {
                    statusid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    statusname = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblstatus", x => x.statusid);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblstatus");

            migrationBuilder.CreateTable(
                name: "tblorderupdated",
                columns: table => new
                {
                    orderid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ad_id = table.Column<int>(type: "int", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    payment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    product_id = table.Column<int>(type: "int", nullable: true),
                    qtys = table.Column<int>(type: "int", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    totalpay = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblorderupdated", x => x.orderid);
                    table.ForeignKey(
                        name: "FK_tblorderupdated_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblorderupdated_tblAddresses_ad_id",
                        column: x => x.ad_id,
                        principalTable: "tblAddresses",
                        principalColumn: "ad_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblorderupdated_tblproduct_product_id",
                        column: x => x.product_id,
                        principalTable: "tblproduct",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblorderupdated_ad_id",
                table: "tblorderupdated",
                column: "ad_id");

            migrationBuilder.CreateIndex(
                name: "IX_tblorderupdated_Id",
                table: "tblorderupdated",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_tblorderupdated_product_id",
                table: "tblorderupdated",
                column: "product_id");
        }
    }
}
