using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProdutosApp.Migrations
{
    /// <inheritdoc />
    public partial class AddTable3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderProduts_Orders_OrdersId",
                table: "OrderProduts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderProduts_Products_ProductsId",
                table: "OrderProduts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderProduts",
                table: "OrderProduts");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "OrderProduts",
                newName: "OrderProducts");

            migrationBuilder.RenameIndex(
                name: "IX_OrderProduts_ProductsId",
                table: "OrderProducts",
                newName: "IX_OrderProducts_ProductsId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Total",
                table: "Orders",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderProducts",
                table: "OrderProducts",
                columns: new[] { "OrdersId", "ProductsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_Orders_OrdersId",
                table: "OrderProducts",
                column: "OrdersId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_Products_ProductsId",
                table: "OrderProducts",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducts_Orders_OrdersId",
                table: "OrderProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducts_Products_ProductsId",
                table: "OrderProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderProducts",
                table: "OrderProducts");

            migrationBuilder.RenameTable(
                name: "OrderProducts",
                newName: "OrderProduts");

            migrationBuilder.RenameIndex(
                name: "IX_OrderProducts_ProductsId",
                table: "OrderProduts",
                newName: "IX_OrderProduts_ProductsId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Total",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Orders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderProduts",
                table: "OrderProduts",
                columns: new[] { "OrdersId", "ProductsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProduts_Orders_OrdersId",
                table: "OrderProduts",
                column: "OrdersId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProduts_Products_ProductsId",
                table: "OrderProduts",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
