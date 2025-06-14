using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class SmartRealEstatePricingModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Estates_RelatedEstateId",
                table: "Notifications");

            migrationBuilder.DropTable(
                name: "Estates");

            migrationBuilder.CreateTable(
                name: "SmartRealEstatePricings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Zone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PropertyType = table.Column<int>(type: "int", nullable: false),
                    RealEstateOperationType = table.Column<int>(type: "int", nullable: true),
                    ConstructionYear = table.Column<int>(type: "int", nullable: false),
                    TotalFloors = table.Column<int>(type: "int", nullable: false),
                    UnitsPerFloor = table.Column<int>(type: "int", nullable: false),
                    Area = table.Column<double>(type: "float", nullable: false),
                    BathroomCount = table.Column<int>(type: "int", nullable: false),
                    RoomCount = table.Column<int>(type: "int", nullable: false),
                    FloorNumber = table.Column<int>(type: "int", nullable: false),
                    NaturalLight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PricePerSquareMeter = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PriceingWithAi = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DocumentType = table.Column<int>(type: "int", nullable: false),
                    HasStorage = table.Column<bool>(type: "bit", nullable: false),
                    HasTerrace = table.Column<bool>(type: "bit", nullable: false),
                    HasParking = table.Column<bool>(type: "bit", nullable: false),
                    HasElevator = table.Column<bool>(type: "bit", nullable: false),
                    HasSauna = table.Column<bool>(type: "bit", nullable: false),
                    HasJacuzzi = table.Column<bool>(type: "bit", nullable: false),
                    HasRoofGarden = table.Column<bool>(type: "bit", nullable: false),
                    HasPool = table.Column<bool>(type: "bit", nullable: false),
                    HasLobby = table.Column<bool>(type: "bit", nullable: false),
                    GreeneryLevel = table.Column<float>(type: "real", nullable: false),
                    PassageWidth = table.Column<float>(type: "real", nullable: false),
                    IsModernTexture = table.Column<bool>(type: "bit", nullable: false),
                    Neighborhood = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartRealEstatePricings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmartRealEstatePricings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SmartRealEstatePricings_UserId",
                table: "SmartRealEstatePricings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_SmartRealEstatePricings_RelatedEstateId",
                table: "Notifications",
                column: "RelatedEstateId",
                principalTable: "SmartRealEstatePricings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_SmartRealEstatePricings_RelatedEstateId",
                table: "Notifications");

            migrationBuilder.DropTable(
                name: "SmartRealEstatePricings");

            migrationBuilder.CreateTable(
                name: "Estates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Area = table.Column<double>(type: "float", nullable: false),
                    BathroomCount = table.Column<int>(type: "int", nullable: false),
                    ConstructionYear = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DocumentType = table.Column<int>(type: "int", nullable: false),
                    FloorNumber = table.Column<int>(type: "int", nullable: false),
                    GreeneryLevel = table.Column<float>(type: "real", nullable: false),
                    HasElevator = table.Column<bool>(type: "bit", nullable: false),
                    HasJacuzzi = table.Column<bool>(type: "bit", nullable: false),
                    HasLobby = table.Column<bool>(type: "bit", nullable: false),
                    HasParking = table.Column<bool>(type: "bit", nullable: false),
                    HasPool = table.Column<bool>(type: "bit", nullable: false),
                    HasRoofGarden = table.Column<bool>(type: "bit", nullable: false),
                    HasSauna = table.Column<bool>(type: "bit", nullable: false),
                    HasStorage = table.Column<bool>(type: "bit", nullable: false),
                    HasTerrace = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsModernTexture = table.Column<bool>(type: "bit", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameNeighborhood = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NaturalLight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Neighborhood = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PassageWidth = table.Column<float>(type: "real", nullable: false),
                    PricePerSquareMeter = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PropertyType = table.Column<int>(type: "int", nullable: false),
                    RealEstateOperationType = table.Column<int>(type: "int", nullable: false),
                    RoomCount = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalFloors = table.Column<int>(type: "int", nullable: false),
                    UnitsPerFloor = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Estates_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Estates_UserId",
                table: "Estates",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Estates_RelatedEstateId",
                table: "Notifications",
                column: "RelatedEstateId",
                principalTable: "Estates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
