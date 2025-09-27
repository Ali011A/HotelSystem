using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_HotelStaff_InOfferEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StaffId",
                table: "Offers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Offers_StaffId",
                table: "Offers",
                column: "StaffId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_HotelStaffs_StaffId",
                table: "Offers",
                column: "StaffId",
                principalTable: "HotelStaffs",
                principalColumn: "UserId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_HotelStaffs_StaffId",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Offers_StaffId",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "StaffId",
                table: "Offers");
        }
    }
}
