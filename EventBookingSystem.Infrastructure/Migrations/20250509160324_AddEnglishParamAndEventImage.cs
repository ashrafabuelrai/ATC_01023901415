using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventBookingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEnglishParamAndEventImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Events",
                newName: "VenueEN");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEN",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameEN",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameEN",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Eventmages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eventmages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Eventmages_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Eventmages_EventId",
                table: "Eventmages",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Eventmages_Id",
                table: "Eventmages",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Eventmages");

            migrationBuilder.DropColumn(
                name: "DescriptionEN",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "NameEN",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "NameEN",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "VenueEN",
                table: "Events",
                newName: "ImageUrl");
        }
    }
}
