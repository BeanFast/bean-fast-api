using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditableEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menu_User_Creater",
                table: "Menu");

            migrationBuilder.DropForeignKey(
                name: "FK_Menu_User_Updater",
                table: "Menu");

            migrationBuilder.RenameColumn(
                name: "CreaterId",
                table: "Menu",
                newName: "CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Menu_CreaterId",
                table: "Menu",
                newName: "IX_Menu_CreatorId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "User",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "User",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "User",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdaterId",
                table: "User",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Session",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "Session",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Session",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdaterId",
                table: "Session",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "School",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "School",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "School",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdaterId",
                table: "School",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "OrderActivity",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "OrderActivity",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "OrderActivity",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdaterId",
                table: "OrderActivity",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Order",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "Order",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Order",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdaterId",
                table: "Order",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Menu",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Menu",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "LoyaltyCard",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "LoyaltyCard",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "LoyaltyCard",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdaterId",
                table: "LoyaltyCard",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Location",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "Location",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Location",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdaterId",
                table: "Location",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Kitchen",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "Kitchen",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Kitchen",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdaterId",
                table: "Kitchen",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Gift",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "Gift",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Gift",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdaterId",
                table: "Gift",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Game",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "Game",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Game",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdaterId",
                table: "Game",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Food",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "Food",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Food",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdaterId",
                table: "Food",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ExchangeGift",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "ExchangeGift",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "ExchangeGift",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdaterId",
                table: "ExchangeGift",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Combo",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "Combo",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Combo",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdaterId",
                table: "Combo",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Category",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "Category",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Category",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdaterId",
                table: "Category",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "CardType",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "CardType",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "CardType",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdaterId",
                table: "CardType",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Area",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "Area",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Area",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdaterId",
                table: "Area",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_CreatorId",
                table: "User",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_User_UpdaterId",
                table: "User",
                column: "UpdaterId");

            migrationBuilder.CreateIndex(
                name: "IX_Session_CreatorId",
                table: "Session",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Session_UpdaterId",
                table: "Session",
                column: "UpdaterId");

            migrationBuilder.CreateIndex(
                name: "IX_School_CreatorId",
                table: "School",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_School_UpdaterId",
                table: "School",
                column: "UpdaterId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderActivity_CreatorId",
                table: "OrderActivity",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderActivity_UpdaterId",
                table: "OrderActivity",
                column: "UpdaterId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CreatorId",
                table: "Order",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_UpdaterId",
                table: "Order",
                column: "UpdaterId");

            migrationBuilder.CreateIndex(
                name: "IX_LoyaltyCard_CreatorId",
                table: "LoyaltyCard",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_LoyaltyCard_UpdaterId",
                table: "LoyaltyCard",
                column: "UpdaterId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_CreatorId",
                table: "Location",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_UpdaterId",
                table: "Location",
                column: "UpdaterId");

            migrationBuilder.CreateIndex(
                name: "IX_Kitchen_CreatorId",
                table: "Kitchen",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Kitchen_UpdaterId",
                table: "Kitchen",
                column: "UpdaterId");

            migrationBuilder.CreateIndex(
                name: "IX_Gift_CreatorId",
                table: "Gift",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Gift_UpdaterId",
                table: "Gift",
                column: "UpdaterId");

            migrationBuilder.CreateIndex(
                name: "IX_Game_CreatorId",
                table: "Game",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Game_UpdaterId",
                table: "Game",
                column: "UpdaterId");

            migrationBuilder.CreateIndex(
                name: "IX_Food_CreatorId",
                table: "Food",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Food_UpdaterId",
                table: "Food",
                column: "UpdaterId");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeGift_CreatorId",
                table: "ExchangeGift",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeGift_UpdaterId",
                table: "ExchangeGift",
                column: "UpdaterId");

            migrationBuilder.CreateIndex(
                name: "IX_Combo_CreatorId",
                table: "Combo",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Combo_UpdaterId",
                table: "Combo",
                column: "UpdaterId");

            migrationBuilder.CreateIndex(
                name: "IX_Category_CreatorId",
                table: "Category",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Category_UpdaterId",
                table: "Category",
                column: "UpdaterId");

            migrationBuilder.CreateIndex(
                name: "IX_CardType_CreatorId",
                table: "CardType",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_CardType_UpdaterId",
                table: "CardType",
                column: "UpdaterId");

            migrationBuilder.CreateIndex(
                name: "IX_Area_CreatorId",
                table: "Area",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Area_UpdaterId",
                table: "Area",
                column: "UpdaterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Area_User_CreatorId",
                table: "Area",
                column: "CreatorId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Area_User_UpdaterId",
                table: "Area",
                column: "UpdaterId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CardType_User_CreatorId",
                table: "CardType",
                column: "CreatorId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CardType_User_UpdaterId",
                table: "CardType",
                column: "UpdaterId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_User_CreatorId",
                table: "Category",
                column: "CreatorId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_User_UpdaterId",
                table: "Category",
                column: "UpdaterId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Combo_User_CreatorId",
                table: "Combo",
                column: "CreatorId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Combo_User_UpdaterId",
                table: "Combo",
                column: "UpdaterId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangeGift_User_CreatorId",
                table: "ExchangeGift",
                column: "CreatorId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangeGift_User_UpdaterId",
                table: "ExchangeGift",
                column: "UpdaterId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Food_User_CreatorId",
                table: "Food",
                column: "CreatorId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Food_User_UpdaterId",
                table: "Food",
                column: "UpdaterId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_User_CreatorId",
                table: "Game",
                column: "CreatorId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_User_UpdaterId",
                table: "Game",
                column: "UpdaterId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Gift_User_CreatorId",
                table: "Gift",
                column: "CreatorId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Gift_User_UpdaterId",
                table: "Gift",
                column: "UpdaterId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Kitchen_User_CreatorId",
                table: "Kitchen",
                column: "CreatorId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Kitchen_User_UpdaterId",
                table: "Kitchen",
                column: "UpdaterId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_User_CreatorId",
                table: "Location",
                column: "CreatorId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_User_UpdaterId",
                table: "Location",
                column: "UpdaterId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LoyaltyCard_User_CreatorId",
                table: "LoyaltyCard",
                column: "CreatorId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LoyaltyCard_User_UpdaterId",
                table: "LoyaltyCard",
                column: "UpdaterId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Menu_User_CreatorId",
                table: "Menu",
                column: "CreatorId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Menu_User_UpdaterId",
                table: "Menu",
                column: "UpdaterId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_User_CreatorId",
                table: "Order",
                column: "CreatorId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_User_UpdaterId",
                table: "Order",
                column: "UpdaterId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderActivity_User_CreatorId",
                table: "OrderActivity",
                column: "CreatorId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderActivity_User_UpdaterId",
                table: "OrderActivity",
                column: "UpdaterId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_School_User_CreatorId",
                table: "School",
                column: "CreatorId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_School_User_UpdaterId",
                table: "School",
                column: "UpdaterId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Session_User_CreatorId",
                table: "Session",
                column: "CreatorId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Session_User_UpdaterId",
                table: "Session",
                column: "UpdaterId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_User_User_CreatorId",
                table: "User",
                column: "CreatorId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_User_User_UpdaterId",
                table: "User",
                column: "UpdaterId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Area_User_CreatorId",
                table: "Area");

            migrationBuilder.DropForeignKey(
                name: "FK_Area_User_UpdaterId",
                table: "Area");

            migrationBuilder.DropForeignKey(
                name: "FK_CardType_User_CreatorId",
                table: "CardType");

            migrationBuilder.DropForeignKey(
                name: "FK_CardType_User_UpdaterId",
                table: "CardType");

            migrationBuilder.DropForeignKey(
                name: "FK_Category_User_CreatorId",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_Category_User_UpdaterId",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_Combo_User_CreatorId",
                table: "Combo");

            migrationBuilder.DropForeignKey(
                name: "FK_Combo_User_UpdaterId",
                table: "Combo");

            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeGift_User_CreatorId",
                table: "ExchangeGift");

            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeGift_User_UpdaterId",
                table: "ExchangeGift");

            migrationBuilder.DropForeignKey(
                name: "FK_Food_User_CreatorId",
                table: "Food");

            migrationBuilder.DropForeignKey(
                name: "FK_Food_User_UpdaterId",
                table: "Food");

            migrationBuilder.DropForeignKey(
                name: "FK_Game_User_CreatorId",
                table: "Game");

            migrationBuilder.DropForeignKey(
                name: "FK_Game_User_UpdaterId",
                table: "Game");

            migrationBuilder.DropForeignKey(
                name: "FK_Gift_User_CreatorId",
                table: "Gift");

            migrationBuilder.DropForeignKey(
                name: "FK_Gift_User_UpdaterId",
                table: "Gift");

            migrationBuilder.DropForeignKey(
                name: "FK_Kitchen_User_CreatorId",
                table: "Kitchen");

            migrationBuilder.DropForeignKey(
                name: "FK_Kitchen_User_UpdaterId",
                table: "Kitchen");

            migrationBuilder.DropForeignKey(
                name: "FK_Location_User_CreatorId",
                table: "Location");

            migrationBuilder.DropForeignKey(
                name: "FK_Location_User_UpdaterId",
                table: "Location");

            migrationBuilder.DropForeignKey(
                name: "FK_LoyaltyCard_User_CreatorId",
                table: "LoyaltyCard");

            migrationBuilder.DropForeignKey(
                name: "FK_LoyaltyCard_User_UpdaterId",
                table: "LoyaltyCard");

            migrationBuilder.DropForeignKey(
                name: "FK_Menu_User_CreatorId",
                table: "Menu");

            migrationBuilder.DropForeignKey(
                name: "FK_Menu_User_UpdaterId",
                table: "Menu");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_User_CreatorId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_User_UpdaterId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderActivity_User_CreatorId",
                table: "OrderActivity");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderActivity_User_UpdaterId",
                table: "OrderActivity");

            migrationBuilder.DropForeignKey(
                name: "FK_School_User_CreatorId",
                table: "School");

            migrationBuilder.DropForeignKey(
                name: "FK_School_User_UpdaterId",
                table: "School");

            migrationBuilder.DropForeignKey(
                name: "FK_Session_User_CreatorId",
                table: "Session");

            migrationBuilder.DropForeignKey(
                name: "FK_Session_User_UpdaterId",
                table: "Session");

            migrationBuilder.DropForeignKey(
                name: "FK_User_User_CreatorId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_User_UpdaterId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_CreatorId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_UpdaterId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Session_CreatorId",
                table: "Session");

            migrationBuilder.DropIndex(
                name: "IX_Session_UpdaterId",
                table: "Session");

            migrationBuilder.DropIndex(
                name: "IX_School_CreatorId",
                table: "School");

            migrationBuilder.DropIndex(
                name: "IX_School_UpdaterId",
                table: "School");

            migrationBuilder.DropIndex(
                name: "IX_OrderActivity_CreatorId",
                table: "OrderActivity");

            migrationBuilder.DropIndex(
                name: "IX_OrderActivity_UpdaterId",
                table: "OrderActivity");

            migrationBuilder.DropIndex(
                name: "IX_Order_CreatorId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_UpdaterId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_LoyaltyCard_CreatorId",
                table: "LoyaltyCard");

            migrationBuilder.DropIndex(
                name: "IX_LoyaltyCard_UpdaterId",
                table: "LoyaltyCard");

            migrationBuilder.DropIndex(
                name: "IX_Location_CreatorId",
                table: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Location_UpdaterId",
                table: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Kitchen_CreatorId",
                table: "Kitchen");

            migrationBuilder.DropIndex(
                name: "IX_Kitchen_UpdaterId",
                table: "Kitchen");

            migrationBuilder.DropIndex(
                name: "IX_Gift_CreatorId",
                table: "Gift");

            migrationBuilder.DropIndex(
                name: "IX_Gift_UpdaterId",
                table: "Gift");

            migrationBuilder.DropIndex(
                name: "IX_Game_CreatorId",
                table: "Game");

            migrationBuilder.DropIndex(
                name: "IX_Game_UpdaterId",
                table: "Game");

            migrationBuilder.DropIndex(
                name: "IX_Food_CreatorId",
                table: "Food");

            migrationBuilder.DropIndex(
                name: "IX_Food_UpdaterId",
                table: "Food");

            migrationBuilder.DropIndex(
                name: "IX_ExchangeGift_CreatorId",
                table: "ExchangeGift");

            migrationBuilder.DropIndex(
                name: "IX_ExchangeGift_UpdaterId",
                table: "ExchangeGift");

            migrationBuilder.DropIndex(
                name: "IX_Combo_CreatorId",
                table: "Combo");

            migrationBuilder.DropIndex(
                name: "IX_Combo_UpdaterId",
                table: "Combo");

            migrationBuilder.DropIndex(
                name: "IX_Category_CreatorId",
                table: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Category_UpdaterId",
                table: "Category");

            migrationBuilder.DropIndex(
                name: "IX_CardType_CreatorId",
                table: "CardType");

            migrationBuilder.DropIndex(
                name: "IX_CardType_UpdaterId",
                table: "CardType");

            migrationBuilder.DropIndex(
                name: "IX_Area_CreatorId",
                table: "Area");

            migrationBuilder.DropIndex(
                name: "IX_Area_UpdaterId",
                table: "Area");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UpdaterId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Session");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Session");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Session");

            migrationBuilder.DropColumn(
                name: "UpdaterId",
                table: "Session");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "School");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "School");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "School");

            migrationBuilder.DropColumn(
                name: "UpdaterId",
                table: "School");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "OrderActivity");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "OrderActivity");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "OrderActivity");

            migrationBuilder.DropColumn(
                name: "UpdaterId",
                table: "OrderActivity");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "UpdaterId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Menu");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Menu");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "LoyaltyCard");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "LoyaltyCard");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "LoyaltyCard");

            migrationBuilder.DropColumn(
                name: "UpdaterId",
                table: "LoyaltyCard");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "UpdaterId",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Kitchen");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Kitchen");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Kitchen");

            migrationBuilder.DropColumn(
                name: "UpdaterId",
                table: "Kitchen");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Gift");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Gift");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Gift");

            migrationBuilder.DropColumn(
                name: "UpdaterId",
                table: "Gift");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "UpdaterId",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "UpdaterId",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ExchangeGift");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "ExchangeGift");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "ExchangeGift");

            migrationBuilder.DropColumn(
                name: "UpdaterId",
                table: "ExchangeGift");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Combo");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Combo");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Combo");

            migrationBuilder.DropColumn(
                name: "UpdaterId",
                table: "Combo");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "UpdaterId",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "CardType");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "CardType");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "CardType");

            migrationBuilder.DropColumn(
                name: "UpdaterId",
                table: "CardType");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Area");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Area");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Area");

            migrationBuilder.DropColumn(
                name: "UpdaterId",
                table: "Area");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Menu",
                newName: "CreaterId");

            migrationBuilder.RenameIndex(
                name: "IX_Menu_CreatorId",
                table: "Menu",
                newName: "IX_Menu_CreaterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Menu_User_Creater",
                table: "Menu",
                column: "CreaterId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Menu_User_Updater",
                table: "Menu",
                column: "UpdaterId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
