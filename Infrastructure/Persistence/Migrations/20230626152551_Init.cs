using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Planets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PhotoUrl = table.Column<string>(type: "text", nullable: false),
                    DistanceFromSunKilometers = table.Column<double>(type: "double precision", nullable: false),
                    MassTonnes = table.Column<double>(type: "double precision", nullable: false),
                    DiameterKilometers = table.Column<double>(type: "double precision", nullable: false),
                    LengthOfDayHours = table.Column<double>(type: "double precision", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planets", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Planets",
                columns: new[] { "Id", "DiameterKilometers", "DistanceFromSunKilometers", "LengthOfDayHours", "MassTonnes", "Name", "PhotoUrl" },
                values: new object[,]
                {
                    { 1, 4879.0, 57910000.0, 4222.6000000000004, 330.19999999999999, "Mercury", "https://dummyimage.com/300x300/000000/ffffff" },
                    { 2, 12104.0, 108200000.0, 2802.0, 4868.5, "Venus", "https://dummyimage.com/300x300/000000/ffffff" },
                    { 3, 12742.0, 149600000.0, 24.0, 5973.6000000000004, "Earth", "https://dummyimage.com/300x300/000000/ffffff" },
                    { 4, 6779.0, 227900000.0, 24.699999999999999, 641.71000000000004, "Mars", "https://dummyimage.com/300x300/000000/ffffff" },
                    { 5, 139820.0, 778500000.0, 9.9000000000000004, 1898600.0, "Jupiter", "https://dummyimage.com/300x300/000000/ffffff" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Planets");
        }
    }
}
