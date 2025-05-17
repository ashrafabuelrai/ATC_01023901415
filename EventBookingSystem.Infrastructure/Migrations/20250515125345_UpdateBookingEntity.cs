using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventBookingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookingEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_UserId",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Bookings",
                newName: "UserName");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_UserId",
                table: "Bookings",
                newName: "IX_Bookings_UserName");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_UserName",
                table: "Bookings",
                column: "UserName",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_UserName",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Bookings",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_UserName",
                table: "Bookings",
                newName: "IX_Bookings_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_UserId",
                table: "Bookings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
