using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class AddDelivererIdInExchangeGift : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DelivererId",
                table: "ExchangeGift",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeGift_DelivererId",
                table: "ExchangeGift",
                column: "DelivererId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangeGift_User_DelivererId",
                table: "ExchangeGift",
                column: "DelivererId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeGift_User_DelivererId",
                table: "ExchangeGift");

            migrationBuilder.DropIndex(
                name: "IX_ExchangeGift_DelivererId",
                table: "ExchangeGift");

            migrationBuilder.DropColumn(
                name: "DelivererId",
                table: "ExchangeGift");
        }
    }
}
