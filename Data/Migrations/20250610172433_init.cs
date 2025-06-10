using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Media");

            migrationBuilder.EnsureSchema(
                name: "UploadedFile");

            migrationBuilder.EnsureSchema(
                name: "ValidationCode");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NationalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastLoginDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Media",
                schema: "Media",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    URL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ObjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RealEstateInfoFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Mantaghe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalFloors = table.Column<int>(type: "int", nullable: false),
                    UnitsPerFloor = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PricePerSquareMeter = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SquareMeters = table.Column<int>(type: "int", nullable: false),
                    Floors = table.Column<int>(type: "int", nullable: false),
                    BuiltArea = table.Column<int>(type: "int", nullable: false),
                    Bedrooms = table.Column<int>(type: "int", nullable: false),
                    Balconies = table.Column<int>(type: "int", nullable: false),
                    HasTelephone = table.Column<bool>(type: "bit", nullable: false),
                    KitchenType = table.Column<int>(type: "int", nullable: false),
                    IsKitchenOpen = table.Column<bool>(type: "bit", nullable: false),
                    BathroomType = table.Column<int>(type: "int", nullable: false),
                    FlooringType = table.Column<int>(type: "int", nullable: false),
                    HasElevator = table.Column<bool>(type: "bit", nullable: false),
                    FacadeType = table.Column<int>(type: "int", nullable: false),
                    DocumentType = table.Column<int>(type: "int", nullable: false),
                    HasParking = table.Column<bool>(type: "bit", nullable: false),
                    HasStorage = table.Column<bool>(type: "bit", nullable: false),
                    BuildingAge = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsFurnished = table.Column<bool>(type: "bit", nullable: false),
                    HasGas = table.Column<bool>(type: "bit", nullable: false),
                    HasVideoIntercom = table.Column<bool>(type: "bit", nullable: false),
                    HasCooler = table.Column<bool>(type: "bit", nullable: false),
                    HasRemoteControlDoor = table.Column<bool>(type: "bit", nullable: false),
                    HasRadiator = table.Column<bool>(type: "bit", nullable: false),
                    HasPackageHeater = table.Column<bool>(type: "bit", nullable: false),
                    IsRenovated = table.Column<bool>(type: "bit", nullable: false),
                    HasJacuzzi = table.Column<bool>(type: "bit", nullable: false),
                    HasSauna = table.Column<bool>(type: "bit", nullable: false),
                    HasPool = table.Column<bool>(type: "bit", nullable: false),
                    HasLobby = table.Column<bool>(type: "bit", nullable: false),
                    HasDuctSplit = table.Column<bool>(type: "bit", nullable: false),
                    HasChiller = table.Column<bool>(type: "bit", nullable: false),
                    HasRoofGarden = table.Column<bool>(type: "bit", nullable: false),
                    HasMasterRoom = table.Column<bool>(type: "bit", nullable: false),
                    HasNoElevator = table.Column<bool>(type: "bit", nullable: false),
                    HasSplitAC = table.Column<bool>(type: "bit", nullable: false),
                    HasJanitorService = table.Column<bool>(type: "bit", nullable: false),
                    HasMeetingHall = table.Column<bool>(type: "bit", nullable: false),
                    HasFanCoil = table.Column<bool>(type: "bit", nullable: false),
                    HasGym = table.Column<bool>(type: "bit", nullable: false),
                    HasCCTV = table.Column<bool>(type: "bit", nullable: false),
                    HasEmergencyPower = table.Column<bool>(type: "bit", nullable: false),
                    IsFlat = table.Column<bool>(type: "bit", nullable: false),
                    HasUnderfloorHeating = table.Column<bool>(type: "bit", nullable: false),
                    HasFireAlarm = table.Column<bool>(type: "bit", nullable: false),
                    HasFireExtinguishingSystem = table.Column<bool>(type: "bit", nullable: false),
                    HasCentralVacuum = table.Column<bool>(type: "bit", nullable: false),
                    HasSecurityDoor = table.Column<bool>(type: "bit", nullable: false),
                    HasLaundry = table.Column<bool>(type: "bit", nullable: false),
                    IsSoldWithTenant = table.Column<bool>(type: "bit", nullable: false),
                    HasCentralAntenna = table.Column<bool>(type: "bit", nullable: false),
                    HasBackyard = table.Column<bool>(type: "bit", nullable: false),
                    HasServantService = table.Column<bool>(type: "bit", nullable: false),
                    HasBarbecue = table.Column<bool>(type: "bit", nullable: false),
                    HasElectricalPanel = table.Column<bool>(type: "bit", nullable: false),
                    HasConferenceHall = table.Column<bool>(type: "bit", nullable: false),
                    HasGuardService = table.Column<bool>(type: "bit", nullable: false),
                    HasAirHandler = table.Column<bool>(type: "bit", nullable: false),
                    HasCentralSatellite = table.Column<bool>(type: "bit", nullable: false),
                    HasGarbageChute = table.Column<bool>(type: "bit", nullable: false),
                    HasLobbyManService = table.Column<bool>(type: "bit", nullable: false),
                    HasGuardOrJanitorService = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealEstateInfoFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ValidationCodes",
                schema: "ValidationCode",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValidationCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Estates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NameNeighborhood = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RealEstateOperationType = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConstructionYear = table.Column<int>(type: "int", nullable: false),
                    TotalFloors = table.Column<int>(type: "int", nullable: false),
                    UnitsPerFloor = table.Column<int>(type: "int", nullable: false),
                    Area = table.Column<double>(type: "float", nullable: false),
                    BathroomCount = table.Column<int>(type: "int", nullable: false),
                    RoomCount = table.Column<int>(type: "int", nullable: false),
                    NaturalLight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PricePerSquareMeter = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    PropertyType = table.Column<int>(type: "int", nullable: false),
                    Neighborhood = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "UploadedFiles",
                schema: "UploadedFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Guid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileType = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    IsSecure = table.Column<bool>(type: "bit", nullable: false),
                    IsAccessForOtherPerson = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadedFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UploadedFiles_AspNetUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OtherPeopleAccessUploadedFiles",
                schema: "UploadedFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    UploadedFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherPeopleAccessUploadedFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OtherPeopleAccessUploadedFiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OtherPeopleAccessUploadedFiles_UploadedFiles_UploadedFileId",
                        column: x => x.UploadedFileId,
                        principalSchema: "UploadedFile",
                        principalTable: "UploadedFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Estates_UserId",
                table: "Estates",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OtherPeopleAccessUploadedFiles_UploadedFileId",
                schema: "UploadedFile",
                table: "OtherPeopleAccessUploadedFiles",
                column: "UploadedFileId");

            migrationBuilder.CreateIndex(
                name: "IX_OtherPeopleAccessUploadedFiles_UserId",
                schema: "UploadedFile",
                table: "OtherPeopleAccessUploadedFiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UploadedFiles_CreatedByUserId",
                schema: "UploadedFile",
                table: "UploadedFiles",
                column: "CreatedByUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Estates");

            migrationBuilder.DropTable(
                name: "Media",
                schema: "Media");

            migrationBuilder.DropTable(
                name: "OtherPeopleAccessUploadedFiles",
                schema: "UploadedFile");

            migrationBuilder.DropTable(
                name: "RealEstateInfoFiles");

            migrationBuilder.DropTable(
                name: "ValidationCodes",
                schema: "ValidationCode");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "UploadedFiles",
                schema: "UploadedFile");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
