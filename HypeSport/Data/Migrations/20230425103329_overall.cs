using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HypeSport.Data.Migrations
{
    /// <inheritdoc />
    public partial class overall : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDetail_Order_OrderId",
                table: "CartDetail");

            migrationBuilder.DropIndex(
                name: "IX_CartDetail_OrderId",
                table: "CartDetail");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "CartDetail");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<double>(
                name: "UnitPrice",
                table: "CartDetail",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "CartDetail");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Order",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "CartDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CartDetail_OrderId",
                table: "CartDetail",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetail_Order_OrderId",
                table: "CartDetail",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
