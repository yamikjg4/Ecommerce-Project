using Microsoft.EntityFrameworkCore.Migrations;

namespace E_Commerce.Migrations
{
    public partial class changeovervieww : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblproduct",
                columns: table => new
                {
                    product_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cat_id = table.Column<int>(type: "int", nullable: false),
                    Product_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    product_price = table.Column<float>(type: "real", nullable: false),
                    product_desc = table.Column<string>(type: "nvarchar(800)", maxLength: 800, nullable: true),
                    product_qty = table.Column<int>(type: "int", nullable: false),
                    ImageFile = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblproduct", x => x.product_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblproduct");
        }
    }
}
