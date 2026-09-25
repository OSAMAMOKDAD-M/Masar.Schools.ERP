using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Masar.Schools.ERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccountNameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountNameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountType = table.Column<int>(type: "int", nullable: false),
                    ParentAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Accounts_ParentAccountId",
                        column: x => x.ParentAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AiChatHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    UserMessage = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    AssistantResponse = table.Column<string>(type: "nvarchar(max)", maxLength: 8000, nullable: false),
                    PageContext = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    UserRole = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsSuccessful = table.Column<bool>(type: "bit", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AiChatHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DiscountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DiscountCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DiscountNameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountNameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountType = table.Column<int>(type: "int", nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FixedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxDiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxUsagePerStudent = table.Column<int>(type: "int", nullable: true),
                    CurrentUsageCount = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GradeLevels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameArabic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JournalEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntryNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPosted = table.Column<bool>(type: "bit", nullable: false),
                    PostedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PostedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntryType = table.Column<int>(type: "int", nullable: false),
                    ReferenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReferenceType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalDebit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCredit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NameArabic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CommercialRegistration = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TaxNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ContactPerson = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SupplierCategory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SupplierCategoryArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PaymentTerms = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreditLimit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "SAR"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameArabic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LicenseNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LogoPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondaryColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SubscriptionStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubscriptionEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ZatcaCertificatePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZatcaSecretKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NameArabic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SKU = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Barcode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CategoryArabic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UnitOfMeasure = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UnitOfMeasureArabic = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ReorderLevel = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxStockLevel = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReorderQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AverageCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SellingPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "SAR"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsStockItem = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreferredSupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryItems_Suppliers_PreferredSupplierId",
                        column: x => x.PreferredSupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryItems_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoles_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BackupRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    StorageLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAutomated = table.Column<bool>(type: "bit", nullable: false),
                    ScheduledTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackupRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BackupRecords_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BackupSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DatabaseBackupPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilesBackupPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RetentionDays = table.Column<int>(type: "int", nullable: false),
                    EnableAutoBackup = table.Column<bool>(type: "bit", nullable: false),
                    AutoBackupTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxBackupCount = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackupSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BackupSettings_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Buses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PlateNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Active"),
                    VehicleIdentificationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastMaintenanceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NextMaintenanceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InsuranceExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LicenseExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    TenantId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Buses_Tenants_TenantId1",
                        column: x => x.TenantId1,
                        principalTable: "Tenants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Guardians",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstNameArabic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastNameArabic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullNameArabic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WhatsAppNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Relationship = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelationshipArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Occupation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OccupationArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guardians", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Guardians_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Schools",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameArabic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoorSchoolId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schools_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameArabic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEnglish = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Module = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    MasarRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permissions_AspNetRoles_MasarRoleId",
                        column: x => x.MasarRoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BusIncidents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IncidentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IncidentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Severity = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Medium"),
                    ReportedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResolvedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResolutionNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    TenantId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusIncidents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusIncidents_Buses_BusId",
                        column: x => x.BusId,
                        principalTable: "Buses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusIncidents_Tenants_TenantId1",
                        column: x => x.TenantId1,
                        principalTable: "Tenants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BusRoutes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RouteName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RouteCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartLocation = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EndLocation = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TotalDistance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EstimatedDuration = table.Column<int>(type: "int", nullable: false),
                    Direction = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Outbound"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    TenantId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BusId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusRoutes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusRoutes_Buses_BusId",
                        column: x => x.BusId,
                        principalTable: "Buses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BusRoutes_Tenants_TenantId1",
                        column: x => x.TenantId1,
                        principalTable: "Tenants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BusTrackingLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Speed = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Direction = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsMoving = table.Column<bool>(type: "bit", nullable: false),
                    BatteryLevel = table.Column<int>(type: "int", nullable: true),
                    SignalStrength = table.Column<int>(type: "int", nullable: true),
                    OdometerReading = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusTrackingLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusTrackingLogs_Buses_BusId",
                        column: x => x.BusId,
                        principalTable: "Buses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FuelRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RefuelDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FuelAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OdometerReading = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FuelStation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RecordedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    TenantId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FuelRecords_Buses_BusId",
                        column: x => x.BusId,
                        principalTable: "Buses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FuelRecords_Tenants_TenantId1",
                        column: x => x.TenantId1,
                        principalTable: "Tenants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaintenanceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaintenanceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Routine"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Workshop = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NextMaintenanceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RecordedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    TenantId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceRecords_Buses_BusId",
                        column: x => x.BusId,
                        principalTable: "Buses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaintenanceRecords_Tenants_TenantId1",
                        column: x => x.TenantId1,
                        principalTable: "Tenants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AcademicTerms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TermName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TermNameArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Pending"),
                    StatusArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "قيد الانتظار"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    BiometricAttendanceEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    RiskPredictionEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnrolledStudentsCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ActiveSectionsCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicTerms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcademicTerms_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameArabic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Branches_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CanteenProductExtras",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CanteenProductExtras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CanteenProductExtras_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FinancialPeriods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PeriodName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false),
                    ClosedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClosedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClosingNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialPeriods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialPeriods_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FinancialPeriods_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LeaveTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameArabic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DaysPerYear = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsAutomatic = table.Column<bool>(type: "bit", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaveTypes_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Nationalities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameArabic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nationalities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nationalities_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCategories_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DepartmentName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequiredBy = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Purpose = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PurposeArabic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BudgetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "SAR"),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Draft"),
                    StatusArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "مسودة"),
                    ApprovedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovalNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseRequests_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsGranted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermissions_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RouteStops",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RouteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StopName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StopOrder = table.Column<int>(type: "int", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    EstimatedArrivalTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    Landmark = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    TenantId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteStops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouteStops_BusRoutes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "BusRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RouteStops_Tenants_TenantId1",
                        column: x => x.TenantId1,
                        principalTable: "Tenants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    AcademicTermId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sections_AcademicTerms_AcademicTermId",
                        column: x => x.AcademicTermId,
                        principalTable: "AcademicTerms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TermArchives",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TermId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TermName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TermNameArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ClosingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArchivedStudentsCount = table.Column<int>(type: "int", nullable: false),
                    ArchivedGradesCount = table.Column<int>(type: "int", nullable: false),
                    ArchivedAttendanceRecords = table.Column<int>(type: "int", nullable: false),
                    ArchivedInvoicesTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ArchivedPaymentsTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ClosingNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClosedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ArchiveData = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    ArchiveFilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ArchiveSizeBytes = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusArabic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsArchiveComplete = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    RestoredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RestoredBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TermArchives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TermArchives_AcademicTerms_TermId",
                        column: x => x.TermId,
                        principalTable: "AcademicTerms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TermArchives_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstNameArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastNameArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullNameArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfileImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsSuperAdmin = table.Column<bool>(type: "bit", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FailedLoginAttempts = table.Column<int>(type: "int", nullable: false),
                    LockoutEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
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
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CanteenTables",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TableNumber = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CanteenTables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CanteenTables_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CanteenTables_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryZones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeliveryFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    MinimumOrderAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    EstimatedDeliveryTime = table.Column<int>(type: "int", nullable: false, defaultValue: 30),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryZones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryZones_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DeliveryZones_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameArabic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WarehouseType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    WarehouseTypeArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Manager = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ManagerPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Capacity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CapacityUnit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Warehouses_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Warehouses_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EntityName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FinancialPeriodId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FinancialPeriodId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditLogs_FinancialPeriods_FinancialPeriodId",
                        column: x => x.FinancialPeriodId,
                        principalTable: "FinancialPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditLogs_FinancialPeriods_FinancialPeriodId1",
                        column: x => x.FinancialPeriodId1,
                        principalTable: "FinancialPeriods",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CanteenProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    Barcode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    AllowStudentBalance = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    AllowEmployeeBalance = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    MaxOrderQuantity = table.Column<int>(type: "int", nullable: false, defaultValue: 10),
                    MinOrderQuantity = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    IsFavorite = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CanteenProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CanteenProducts_ProductCategories_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CanteenProducts_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseRequestItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PurchaseRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InventoryItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EstimatedPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EstimatedTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PreferredSupplier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseRequestItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseRequestItems_InventoryItems_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalTable: "InventoryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseRequestItems_PurchaseRequests_PurchaseRequestId",
                        column: x => x.PurchaseRequestId,
                        principalTable: "PurchaseRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        onDelete: ReferentialAction.Cascade);
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
                        onDelete: ReferentialAction.Cascade);
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstNameArabic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastNameArabic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullNameArabic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobTitleArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartmentArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TerminationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BankAccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfileImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FingerprintTemplate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GenderArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NationalityArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ContractEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsResigned = table.Column<bool>(type: "bit", nullable: false),
                    ResignDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResignReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResignReasonArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaritalStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaritalStatusArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PassportNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Education = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EducationArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Specialty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecialtyArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BiometricId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MasarUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_AspNetUsers_MasarUserId",
                        column: x => x.MasarUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Employees_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsGranted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPermissions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PickUpPoints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeliveryZoneId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickUpPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PickUpPoints_DeliveryZones_DeliveryZoneId",
                        column: x => x.DeliveryZoneId,
                        principalTable: "DeliveryZones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PickUpPoints_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InventoryTransfers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransferNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FromWarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ToWarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransferDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Draft"),
                    StatusArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "مسودة"),
                    TotalValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "SAR"),
                    Reason = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ReasonArabic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PerformedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryTransfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryTransfers_Warehouses_FromWarehouseId",
                        column: x => x.FromWarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryTransfers_Warehouses_ToWarehouseId",
                        column: x => x.ToWarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpectedDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Draft"),
                    StatusArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "مسودة"),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemainingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "SAR"),
                    PaymentTerms = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DeliveryTerms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotesArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedByName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockAdjustments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdjustmentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AdjustmentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AdjustmentTypeArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdjustmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ReasonArabic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "SAR"),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Pending"),
                    StatusArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "قيد الانتظار"),
                    ApprovedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovalNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    WarehouseId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockAdjustments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockAdjustments_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockAdjustments_Warehouses_WarehouseId1",
                        column: x => x.WarehouseId1,
                        principalTable: "Warehouses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StockTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransactionNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TransactionTypeArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    InventoryItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NegativeQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UnitCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "SAR"),
                    BalanceBefore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BalanceAfter = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReceiverId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReceiverName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PerformedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    InventoryItemId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WarehouseId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockTransactions_InventoryItems_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalTable: "InventoryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockTransactions_InventoryItems_InventoryItemId1",
                        column: x => x.InventoryItemId1,
                        principalTable: "InventoryItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockTransactions_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockTransactions_Warehouses_WarehouseId1",
                        column: x => x.WarehouseId1,
                        principalTable: "Warehouses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CanteenProductExtrasMapping",
                columns: table => new
                {
                    ExtrasId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CanteenProductExtrasMapping", x => new { x.ExtrasId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_CanteenProductExtrasMapping_CanteenProductExtras_ExtrasId",
                        column: x => x.ExtrasId,
                        principalTable: "CanteenProductExtras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CanteenProductExtrasMapping_CanteenProducts_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "CanteenProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CanteenProductImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CanteenProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    AltText = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CanteenProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CanteenProductImages_CanteenProducts_CanteenProductId",
                        column: x => x.CanteenProductId,
                        principalTable: "CanteenProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CanteenProductVariants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CanteenProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ExtraPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CanteenProductVariants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CanteenProductVariants_CanteenProducts_CanteenProductId",
                        column: x => x.CanteenProductId,
                        principalTable: "CanteenProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttendanceLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BiometricId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EventAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Direction = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "غير مصنف"),
                    Source = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "يدوي"),
                    DeviceName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendanceLogs_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AttendanceLogs_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AttendancePermits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermitDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DurationHours = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendancePermits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendancePermits_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AttendancePermits_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BusSupervisors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EmergencyContact = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    AssignedBusId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    TenantId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusSupervisors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusSupervisors_Buses_AssignedBusId",
                        column: x => x.AssignedBusId,
                        principalTable: "Buses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusSupervisors_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BusSupervisors_Tenants_TenantId1",
                        column: x => x.TenantId1,
                        principalTable: "Tenants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CashierShifts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OpeningBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    OpeningCardBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    ExpectedCash = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    ActualCash = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    CardPayments = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    StudentBalancePayments = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    EmployeeBalancePayments = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    TotalSales = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    OrderCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    InvoiceCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ShortageOverage = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ClosedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ShortageOverageReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashierShifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashierShifts_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CashierShifts_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CashierShifts_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClassRooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameArabic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    GradeLevelLegacy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SectionLegacy = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    GradeLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Capacity = table.Column<int>(type: "int", nullable: false, defaultValue: 30),
                    CurrentCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ClassTeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassRooms_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ClassRooms_Employees_ClassTeacherId",
                        column: x => x.ClassTeacherId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassRooms_GradeLevels_GradeLevelId",
                        column: x => x.GradeLevelId,
                        principalTable: "GradeLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassRooms_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassRooms_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassRooms_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContractArchives",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OriginalContractNumber = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    JobTitleArabic = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Department = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DepartmentArabic = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ArchiveReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ArchiveReasonArabic = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractArchives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractArchives_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContractArchives_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContractNumber = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    JobTitleArabic = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Department = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DepartmentArabic = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DurationYears = table.Column<int>(type: "int", nullable: false),
                    DurationMonths = table.Column<int>(type: "int", nullable: false),
                    DurationDays = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "ساري"),
                    StatusArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "ساري"),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contracts_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contracts_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LicenseNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LicenseExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LicenseType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    YearsOfExperience = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    AssignedBusId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    TenantId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Drivers_Buses_AssignedBusId",
                        column: x => x.AssignedBusId,
                        principalTable: "Buses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Drivers_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Drivers_Tenants_TenantId1",
                        column: x => x.TenantId1,
                        principalTable: "Tenants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LeaveBalances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LeaveType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LeaveTypeArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FiscalYear = table.Column<int>(type: "int", nullable: false),
                    AnnualAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UsedDays = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemainingDays = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CarriedOverDays = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SpecialAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveBalances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaveBalances_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeaveBalances_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LeaveRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LeaveType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LeaveTypeArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DaysCount = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReasonArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Pending"),
                    StatusArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ManagerNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActionBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttachmentPaths = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotificationSent = table.Column<bool>(type: "bit", nullable: false),
                    NotificationSentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LeaveNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PreviousBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RemainingBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaveRequests_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeaveRequests_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MedicalItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ItemNameArabic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ItemType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ItemTypeArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Manufacturer = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ManufacturerArabic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DrugCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BatchNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ManufacturingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AvailableQuantity = table.Column<int>(type: "int", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UnitArabic = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinimumStockLevel = table.Column<int>(type: "int", nullable: false),
                    ConsumedQuantity = table.Column<int>(type: "int", nullable: false),
                    StorageLocation = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    StorageLocationArabic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RequiredTemperature = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RequiresSpecialStorage = table.Column<bool>(type: "bit", nullable: false),
                    StorageType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StorageTypeArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RecommendedDosage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RecommendedDosageArabic = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SideEffects = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SideEffectsArabic = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DrugInteractions = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DrugInteractionsArabic = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Contraindications = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ContraindicationsArabic = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsExpired = table.Column<bool>(type: "bit", nullable: false),
                    NearExpiry = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    NotesArabic = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    MaxDispenseQuantity = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedByEmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByEmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalItems_Employees_CreatedByEmployeeId",
                        column: x => x.CreatedByEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MedicalItems_Employees_UpdatedByEmployeeId",
                        column: x => x.UpdatedByEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MedicalItems_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayrollArchives",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EmployeeNameArabic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EmployeeCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Basic = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Housing = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Transport = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OtherEarnings = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VariableEarnings = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Deductions = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AbsenceDays = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AbsenceDeduction = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LateDays = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LateDeduction = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GosiEnabled = table.Column<bool>(type: "bit", nullable: false),
                    GosiScheme = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "auto"),
                    GosiCommission = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GosiInKindHousing = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GosiWage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GosiServiceDays = table.Column<int>(type: "int", nullable: false),
                    GosiEmployeePensionRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GosiEmployerPensionRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GosiEmployeeSanedRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GosiEmployerSanedRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GosiOccupationalHazardRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GosiEmployeeContribution = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GosiEmployerContribution = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollArchives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayrollArchives_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollArchives_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayrollProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Basic = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Housing = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Transport = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OtherEarnings = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VariableEarnings = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Deductions = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AbsenceDays = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LateDays = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LateDeduction = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GosiEnabled = table.Column<bool>(type: "bit", nullable: false),
                    GosiScheme = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "auto"),
                    GosiCommission = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GosiInKindHousing = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GosiWage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GosiServiceDays = table.Column<int>(type: "int", nullable: false),
                    GosiEmployeePensionRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GosiEmployerPensionRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GosiEmployeeSanedRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GosiEmployerSanedRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GosiOccupationalHazardRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GosiEmployeeContribution = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GosiEmployerContribution = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Approved = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayrollProfiles_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollProfiles_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResignedEmployees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FullNameArabic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    JobTitleArabic = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Department = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DepartmentArabic = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ContractStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResignDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ReasonArabic = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ServiceYears = table.Column<int>(type: "int", nullable: false),
                    ServiceMonths = table.Column<int>(type: "int", nullable: false),
                    ServiceDays = table.Column<int>(type: "int", nullable: false),
                    GratuityAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResignedEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResignedEmployees_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResignedEmployees_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InventoryTransferItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InventoryTransferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InventoryItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryTransferItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryTransferItems_InventoryItems_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalTable: "InventoryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryTransferItems_InventoryTransfers_InventoryTransferId",
                        column: x => x.InventoryTransferId,
                        principalTable: "InventoryTransfers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseInvoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TaxInvoiceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PurchaseOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemainingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "SAR"),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Pending"),
                    StatusArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "قيد الانتظار"),
                    IsZatcaApproved = table.Column<bool>(type: "bit", nullable: false),
                    ZatcaId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ZatcaApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotesArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseInvoices_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseInvoices_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseInvoices_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PurchaseOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InventoryItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReceivedQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemainingQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FinalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderItems_InventoryItems_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalTable: "InventoryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderItems_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockAdjustmentItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StockAdjustmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InventoryItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AdjustedQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NewBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockAdjustmentItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockAdjustmentItems_InventoryItems_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalTable: "InventoryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockAdjustmentItems_StockAdjustments_StockAdjustmentId",
                        column: x => x.StockAdjustmentId,
                        principalTable: "StockAdjustments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatRooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameArabic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastMessageAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ClassRoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatRooms_ClassRooms_ClassRoomId",
                        column: x => x.ClassRoomId,
                        principalTable: "ClassRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChatRooms_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ChatRooms_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RollCallSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassRoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Period = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PeriodArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SubjectArabic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TeacherNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpectedStudentsCount = table.Column<int>(type: "int", nullable: false),
                    PresentCount = table.Column<int>(type: "int", nullable: false),
                    AbsentCount = table.Column<int>(type: "int", nullable: false),
                    LateCount = table.Column<int>(type: "int", nullable: false),
                    SessionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SessionTypeArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    WhatsAppNotificationsSent = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    WhatsAppNotificationsSentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RollCallSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RollCallSessions_ClassRooms_ClassRoomId",
                        column: x => x.ClassRoomId,
                        principalTable: "ClassRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RollCallSessions_Employees_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstNameArabic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastNameArabic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullNameArabic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BloodType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfileImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NoorStudentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnrollmentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GraduationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsGraduated = table.Column<bool>(type: "bit", nullable: false),
                    GraduationYear = table.Column<int>(type: "int", nullable: true),
                    FinalGPA = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EmergencyContactName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmergencyContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicalNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FingerprintTemplate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSaudiCitizen = table.Column<bool>(type: "bit", nullable: false),
                    PreviousSchool = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreviousSchoolArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransferDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TransferReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicStanding = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecialNeeds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecialNeedsArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsGifted = table.Column<bool>(type: "bit", nullable: false),
                    GiftedProgram = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastMedicalCheckup = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Allergies = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AllergiesArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DietaryRestrictions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DietaryRestrictionsArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ClassRoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GuardianId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Students_ClassRooms_ClassRoomId",
                        column: x => x.ClassRoomId,
                        principalTable: "ClassRooms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Students_Guardians_GuardianId",
                        column: x => x.GuardianId,
                        principalTable: "Guardians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Students_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BusSchedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RouteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SupervisorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TripType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Morning"),
                    DepartureTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ArrivalTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    TenantId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusSchedules_BusRoutes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "BusRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusSchedules_BusSupervisors_SupervisorId",
                        column: x => x.SupervisorId,
                        principalTable: "BusSupervisors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BusSchedules_Buses_BusId",
                        column: x => x.BusId,
                        principalTable: "Buses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusSchedules_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BusSchedules_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BusSchedules_Tenants_TenantId1",
                        column: x => x.TenantId1,
                        principalTable: "Tenants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseInvoiceItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PurchaseInvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InventoryItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FinalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseInvoiceItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseInvoiceItems_InventoryItems_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalTable: "InventoryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseInvoiceItems_PurchaseInvoices_PurchaseInvoiceId",
                        column: x => x.PurchaseInvoiceId,
                        principalTable: "PurchaseInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MessageType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChatRoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReplyToMessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMessages_AspNetUsers_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChatMessages_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChatMessages_ChatMessages_ReplyToMessageId",
                        column: x => x.ReplyToMessageId,
                        principalTable: "ChatMessages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ChatMessages_ChatRooms_ChatRoomId",
                        column: x => x.ChatRoomId,
                        principalTable: "ChatRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatRoomMembers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LeftAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    IsMuted = table.Column<bool>(type: "bit", nullable: false),
                    MutedUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChatRoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRoomMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatRoomMembers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatRoomMembers_ChatRooms_ChatRoomId",
                        column: x => x.ChatRoomId,
                        principalTable: "ChatRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AcademicRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AcademicYear = table.Column<int>(type: "int", nullable: false),
                    GradeLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GradeLevelArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Section = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    SectionArabic = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    GPA = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OverallPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AcademicStanding = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AcademicStandingArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ClassRank = table.Column<int>(type: "int", nullable: true),
                    TotalStudentsInClass = table.Column<int>(type: "int", nullable: true),
                    AbsenceDays = table.Column<int>(type: "int", nullable: true),
                    LateDays = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    AcademicYearStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AcademicYearEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CurrentTerm = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CurrentTermArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcademicRecords_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdmissionApplications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ApplicationType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AcademicYear = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    GradeLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FullNameArabic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FullNameEnglish = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NationalId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PassportNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Religion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CivilId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsSaudiCitizen = table.Column<bool>(type: "bit", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    HasSpecialNeeds = table.Column<bool>(type: "bit", nullable: false),
                    SpecialNeedsDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsGifted = table.Column<bool>(type: "bit", nullable: false),
                    GiftedDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GuardianFirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GuardianLastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GuardianFullNameArabic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Relationship = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RelationshipArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GuardianNationalId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GuardianPhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    GuardianWhatsAppNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    GuardianEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    GuardianOccupation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GuardianOccupationArabic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GuardianWorkplace = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    GuardianWorkAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    GuardianAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PreviousSchool = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PreviousSchoolArabic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PreviousSchoolCity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TransferReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TransferReasonArabic = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ApplicationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubmittedByEmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReviewedByEmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReviewDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RejectionReasonArabic = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    DecisionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConvertedStudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ConversionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WhatsAppNotificationSent = table.Column<bool>(type: "bit", nullable: false),
                    WhatsAppNotificationSentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmailNotificationSent = table.Column<bool>(type: "bit", nullable: false),
                    EmailNotificationSentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdmissionApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdmissionApplications_Employees_ReviewedByEmployeeId",
                        column: x => x.ReviewedByEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdmissionApplications_Employees_SubmittedByEmployeeId",
                        column: x => x.SubmittedByEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdmissionApplications_GradeLevels_GradeLevelId",
                        column: x => x.GradeLevelId,
                        principalTable: "GradeLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdmissionApplications_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdmissionApplications_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdmissionApplications_Students_ConvertedStudentId",
                        column: x => x.ConvertedStudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AlumniRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GraduationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GraduationYear = table.Column<int>(type: "int", nullable: false),
                    FinalGPA = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GradeLevel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GradeLevelArabic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Section = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SectionArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    University = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UniversityArabic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Major = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MajorArabic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UniversityEnrollmentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UniversityGraduationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmploymentStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmploymentStatusArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CompanyNameArabic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    JobTitleArabic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Industry = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IndustryArabic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmploymentStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PersonalEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PersonalPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    LinkedInProfile = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CurrentAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CurrentAddressArabic = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Achievements = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    AchievementsArabic = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NotesArabic = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    LastContactDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedByEmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlumniRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlumniRecords_Employees_CreatedByEmployeeId",
                        column: x => x.CreatedByEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AlumniRecords_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AlumniRecords_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AttendanceRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckInTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckOutTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAutomated = table.Column<bool>(type: "bit", nullable: false),
                    WhatsAppNotificationSent = table.Column<bool>(type: "bit", nullable: false),
                    WhatsAppNotificationSentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Method = table.Column<int>(type: "int", nullable: false),
                    DeviceLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhotoProof = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsOverride = table.Column<bool>(type: "bit", nullable: false),
                    OverrideReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OverrideBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LateMinutes = table.Column<int>(type: "int", nullable: true),
                    LateType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsExcused = table.Column<bool>(type: "bit", nullable: false),
                    ExcuseReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExcuseSubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ClassRoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendanceRecords_ClassRooms_ClassRoomId",
                        column: x => x.ClassRoomId,
                        principalTable: "ClassRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AttendanceRecords_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AttendanceRecords_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CanteenOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CashierShiftId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CanteenTableId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeliveryZoneId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PickUpPointId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeliveryEmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeliveryFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StudentBalanceDeduction = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    CustomerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CustomerPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DeliveryAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PartySize = table.Column<int>(type: "int", nullable: true),
                    EstimatedDeliveryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualDeliveryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CanteenOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CanteenOrders_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CanteenOrders_CanteenTables_CanteenTableId",
                        column: x => x.CanteenTableId,
                        principalTable: "CanteenTables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CanteenOrders_CashierShifts_CashierShiftId",
                        column: x => x.CashierShiftId,
                        principalTable: "CashierShifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CanteenOrders_DeliveryZones_DeliveryZoneId",
                        column: x => x.DeliveryZoneId,
                        principalTable: "DeliveryZones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CanteenOrders_Employees_DeliveryEmployeeId",
                        column: x => x.DeliveryEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CanteenOrders_PickUpPoints_PickUpPointId",
                        column: x => x.PickUpPointId,
                        principalTable: "PickUpPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CanteenOrders_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CanteenOrders_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubjectArabic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MidtermScore = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FinalScore = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalScore = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MaxScore = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GradeLetter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Term = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicYear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grades_Employees_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Grades_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GraduationClearanceRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GraduationYear = table.Column<int>(type: "int", nullable: false),
                    GradeLevel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GradeLevelArabic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ClearanceStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ClearanceStatusArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FinancialCleared = table.Column<bool>(type: "bit", nullable: false),
                    FinancialClearanceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FinancialClearanceNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FinancialClearanceNotesArabic = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    OutstandingBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AdministrativeCleared = table.Column<bool>(type: "bit", nullable: false),
                    AdministrativeClearanceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AdministrativeClearanceNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AdministrativeClearanceNotesArabic = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ReturnedLibraryBooks = table.Column<bool>(type: "bit", nullable: false),
                    ReturnedEquipment = table.Column<bool>(type: "bit", nullable: false),
                    AcademicCleared = table.Column<bool>(type: "bit", nullable: false),
                    AcademicClearanceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AcademicClearanceNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AcademicClearanceNotesArabic = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AllGradesRecorded = table.Column<bool>(type: "bit", nullable: false),
                    AllRequirementsMet = table.Column<bool>(type: "bit", nullable: false),
                    ClinicCleared = table.Column<bool>(type: "bit", nullable: false),
                    ClinicClearanceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClinicClearanceNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ClinicClearanceNotesArabic = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FinalApproval = table.Column<bool>(type: "bit", nullable: false),
                    FinalApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedByEmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApprovedByEmployeeName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FinalApprovalNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FinalApprovalNotesArabic = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    NotesArabic = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedByEmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraduationClearanceRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GraduationClearanceRecords_Employees_CreatedByEmployeeId",
                        column: x => x.CreatedByEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GraduationClearanceRecords_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZatcaInvoiceUuid = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ZatcaQrCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZatcaHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZatcaSubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ZatcaCompliance = table.Column<bool>(type: "bit", nullable: false),
                    ZatcaStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZatcaErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invoices_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JournalEntryLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Debit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LineNumber = table.Column<int>(type: "int", nullable: false),
                    AccountId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalEntryLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JournalEntryLines_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JournalEntryLines_Accounts_AccountId1",
                        column: x => x.AccountId1,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JournalEntryLines_JournalEntries_EntryId",
                        column: x => x.EntryId,
                        principalTable: "JournalEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JournalEntryLines_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RiskAlertHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PreviousRiskLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreviousRiskScore = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    NewRiskLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewRiskScore = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    AlertType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TransitionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AlertMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Recipient = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Channel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotificationId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskAlertHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskAlertHistories_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RollCallRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StatusArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AbsenceReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AbsenceReasonArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LateMinutes = table.Column<int>(type: "int", nullable: true),
                    GuardianNotificationSent = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    GuardianNotificationSentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NotificationType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsModified = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModificationReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecordingMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AttendedFullPeriod = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    EarlyDepartureTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EarlyDepartureReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsExcused = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ExcuseApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExcuseApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RollCallRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RollCallRecords_RollCallSessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "RollCallSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RollCallRecords_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TotalBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OutstandingBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastPaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastInvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsVatExempt = table.Column<bool>(type: "bit", nullable: false),
                    VatExemptionCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentAccounts_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentAccounts_Students_StudentId1",
                        column: x => x.StudentId1,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StudentDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DocumentTypeArabic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OriginalFileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ThumbnailPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    MimeType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VerifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerificationNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CategoryArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsConfidential = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    RetentionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    PreviousDocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StatusArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentDocuments_StudentDocuments_PreviousDocumentId",
                        column: x => x.PreviousDocumentId,
                        principalTable: "StudentDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentDocuments_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentHealthProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BloodType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    RhFactor = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Allergies = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AllergiesArabic = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ProhibitedFoods = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ProhibitedFoodsArabic = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ChronicDiseases = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ChronicDiseasesArabic = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PhysicalDisabilities = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PhysicalDisabilitiesArabic = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Vaccinations = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    VaccinationsArabic = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    LastComprehensiveCheckup = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DietaryRestrictions = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DietaryRestrictionsArabic = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AdditionalNotes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    AdditionalNotesArabic = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    RequiresSpecialCare = table.Column<bool>(type: "bit", nullable: false),
                    SpecialCareType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SpecialCareTypeArabic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TreatingPhysician = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TreatingPhysicianArabic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PhysicianPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PreferredHospital = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    InsuranceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InsuranceCompany = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ProfileCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByEmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentHealthProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentHealthProfiles_Employees_CreatedByEmployeeId",
                        column: x => x.CreatedByEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentHealthProfiles_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentRiskScores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RiskScore = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    AttendanceRisk = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    AcademicRisk = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    FinancialRisk = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    AttendanceWeight = table.Column<decimal>(type: "decimal(5,4)", nullable: false),
                    AcademicWeight = table.Column<decimal>(type: "decimal(5,4)", nullable: false),
                    FinancialWeight = table.Column<decimal>(type: "decimal(5,4)", nullable: false),
                    RiskLevel = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CalculationVersion = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DataCompleteness = table.Column<decimal>(type: "decimal(5,4)", nullable: false),
                    PrimaryRiskFactors = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Recommendations = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CalculatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CalculatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentRiskScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentRiskScores_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusAttendances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttendanceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AttendanceType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Pickup"),
                    StopId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CheckInTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckOutTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Present"),
                    RecordedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    TenantId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BusId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusAttendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusAttendances_BusSchedules_BusScheduleId",
                        column: x => x.BusScheduleId,
                        principalTable: "BusSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusAttendances_Buses_BusId",
                        column: x => x.BusId,
                        principalTable: "Buses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BusAttendances_RouteStops_StopId",
                        column: x => x.StopId,
                        principalTable: "RouteStops",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BusAttendances_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusAttendances_Tenants_TenantId1",
                        column: x => x.TenantId1,
                        principalTable: "Tenants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StudentTransportSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RouteStopId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubscriptionStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubscriptionEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MonthlyFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Pending"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    TenantId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BusRouteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentTransportSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentTransportSubscriptions_BusRoutes_BusRouteId",
                        column: x => x.BusRouteId,
                        principalTable: "BusRoutes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentTransportSubscriptions_BusSchedules_BusScheduleId",
                        column: x => x.BusScheduleId,
                        principalTable: "BusSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentTransportSubscriptions_RouteStops_RouteStopId",
                        column: x => x.RouteStopId,
                        principalTable: "RouteStops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentTransportSubscriptions_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentTransportSubscriptions_Tenants_TenantId1",
                        column: x => x.TenantId1,
                        principalTable: "Tenants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AdmissionExams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdmissionApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InterviewStatus = table.Column<int>(type: "int", nullable: false),
                    InterviewDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InterviewTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    InterviewLocation = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    InterviewerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InterviewScore = table.Column<int>(type: "int", nullable: true),
                    InterviewNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ExamStatus = table.Column<int>(type: "int", nullable: false),
                    ExamDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExamTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    ExamLocation = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ExaminerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ArabicScore = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MathScore = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ScienceScore = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EnglishScore = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalScore = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PassingScore = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PassedExam = table.Column<bool>(type: "bit", nullable: true),
                    ExamNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    MedicalNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    BehavioralNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FinalRecommendation = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FinalRecommendationArabic = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    EvaluationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EvaluatedByEmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InterviewWhatsAppSent = table.Column<bool>(type: "bit", nullable: false),
                    InterviewWhatsAppSentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExamWhatsAppSent = table.Column<bool>(type: "bit", nullable: false),
                    ExamWhatsAppSentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdmissionExams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdmissionExams_AdmissionApplications_AdmissionApplicationId",
                        column: x => x.AdmissionApplicationId,
                        principalTable: "AdmissionApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdmissionExams_Employees_EvaluatedByEmployeeId",
                        column: x => x.EvaluatedByEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdmissionExams_Employees_ExaminerId",
                        column: x => x.ExaminerId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdmissionExams_Employees_InterviewerId",
                        column: x => x.InterviewerId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdmissionExams_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdmissionApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentType = table.Column<int>(type: "int", nullable: false),
                    DocumentName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OriginalFileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    VerifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VerifiedByEmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VerificationNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UploadedByEmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationDocuments_AdmissionApplications_AdmissionApplicationId",
                        column: x => x.AdmissionApplicationId,
                        principalTable: "AdmissionApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationDocuments_Employees_UploadedByEmployeeId",
                        column: x => x.UploadedByEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicationDocuments_Employees_VerifiedByEmployeeId",
                        column: x => x.VerifiedByEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GraduationDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AlumniRecordId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DocumentTypeArabic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DocumentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DescriptionArabic = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FileType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: true),
                    IsIssued = table.Column<bool>(type: "bit", nullable: false),
                    IsDelivered = table.Column<bool>(type: "bit", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveryMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReceivedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ReceivedByArabic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FinancialCleared = table.Column<bool>(type: "bit", nullable: false),
                    FinancialClearanceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FinancialClearanceNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AdministrativeCleared = table.Column<bool>(type: "bit", nullable: false),
                    AdministrativeClearanceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AdministrativeClearanceNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    NotesArabic = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedByEmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraduationDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GraduationDocuments_AlumniRecords_AlumniRecordId",
                        column: x => x.AlumniRecordId,
                        principalTable: "AlumniRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GraduationDocuments_Employees_CreatedByEmployeeId",
                        column: x => x.CreatedByEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CanteenOrderItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CanteenOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CanteenProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VariantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LineTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CanteenOrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CanteenOrderItems_CanteenOrders_CanteenOrderId",
                        column: x => x.CanteenOrderId,
                        principalTable: "CanteenOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CanteenOrderItems_CanteenProductVariants_VariantId",
                        column: x => x.VariantId,
                        principalTable: "CanteenProductVariants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CanteenOrderItems_CanteenProducts_CanteenProductId",
                        column: x => x.CanteenProductId,
                        principalTable: "CanteenProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CanteenPayments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CanteenOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Method = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionRef = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProcessedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StudentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmployeeNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BalanceBefore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BalanceAfter = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CanteenPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CanteenPayments_CanteenOrders_CanteenOrderId",
                        column: x => x.CanteenOrderId,
                        principalTable: "CanteenOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionArabic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LineTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ZatcaItemCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceItems_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoicePayments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoicePayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoicePayments_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentInvoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UUID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StudentAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OutstandingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    InvoiceType = table.Column<int>(type: "int", nullable: false),
                    ZatcaStatus = table.Column<int>(type: "int", nullable: false),
                    XmlPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QrCodeBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CryptographicStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVatExempt = table.Column<bool>(type: "bit", nullable: false),
                    VatExemptionCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentInvoices_StudentAccounts_StudentAccountId",
                        column: x => x.StudentAccountId,
                        principalTable: "StudentAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentActivityLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ActivityType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActivityTypeArabic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentActivityLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentActivityLogs_StudentDocuments_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "StudentDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentActivityLogs_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClinicVisits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttendingEmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VisitDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    VisitReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VisitReasonArabic = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VisitType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VisitTypeArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Symptoms = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SymptomsArabic = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Temperature = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BloodPressureSystolic = table.Column<int>(type: "int", nullable: true),
                    BloodPressureDiastolic = table.Column<int>(type: "int", nullable: true),
                    HeartRate = table.Column<int>(type: "int", nullable: true),
                    RespiratoryRate = table.Column<int>(type: "int", nullable: true),
                    OxygenSaturation = table.Column<int>(type: "int", nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Height = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PreliminaryDiagnosis = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PreliminaryDiagnosisArabic = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Treatment = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    TreatmentArabic = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    MedicationPrescribed = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    MedicationPrescribedArabic = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Dosage = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DosageArabic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Duration = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DurationArabic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PatientCondition = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PatientConditionArabic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ReturnedToClass = table.Column<bool>(type: "bit", nullable: false),
                    ReferredToHospital = table.Column<bool>(type: "bit", nullable: false),
                    SentHome = table.Column<bool>(type: "bit", nullable: false),
                    ReferredHospital = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ReferredHospitalArabic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ReferralReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ReferralReasonArabic = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsEmergency = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NotesArabic = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WhatsAppNotificationSent = table.Column<bool>(type: "bit", nullable: false),
                    WhatsAppNotificationSentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HealthProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicVisits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClinicVisits_Employees_AttendingEmployeeId",
                        column: x => x.AttendingEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClinicVisits_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClinicVisits_StudentHealthProfiles_HealthProfileId",
                        column: x => x.HealthProfileId,
                        principalTable: "StudentHealthProfiles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClinicVisits_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CanteenOrderItemExtras",
                columns: table => new
                {
                    CanteenOrderItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CanteenProductExtraId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CanteenOrderItemExtras", x => new { x.CanteenOrderItemId, x.CanteenProductExtraId });
                    table.ForeignKey(
                        name: "FK_CanteenOrderItemExtras_CanteenOrderItems_CanteenOrderItemId",
                        column: x => x.CanteenOrderItemId,
                        principalTable: "CanteenOrderItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CanteenOrderItemExtras_CanteenProductExtras_CanteenProductExtraId",
                        column: x => x.CanteenProductExtraId,
                        principalTable: "CanteenProductExtras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceLineItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LineTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LineNet = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ItemCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LineNumber = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceLineItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceLineItems_StudentInvoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "StudentInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentDiscounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentDiscountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DiscountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AppliedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentDiscounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentDiscounts_Discounts_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentDiscounts_StudentAccounts_StudentAccountId",
                        column: x => x.StudentAccountId,
                        principalTable: "StudentAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentDiscounts_StudentInvoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "StudentInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentPayments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StudentAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ReceiptNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiptPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QrCodeBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReceivedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentPayments_StudentAccounts_StudentAccountId",
                        column: x => x.StudentAccountId,
                        principalTable: "StudentAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentPayments_StudentInvoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "StudentInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MedicalExcuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicVisitId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExcuseType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExcuseTypeArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ReasonArabic = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    DescriptionArabic = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumberOfDays = table.Column<int>(type: "int", nullable: false),
                    IsDeductible = table.Column<bool>(type: "bit", nullable: false),
                    PhysicalActivityRecommendations = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PhysicalActivityRecommendationsArabic = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DietaryRecommendations = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DietaryRecommendationsArabic = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsPhysicalActivityRestricted = table.Column<bool>(type: "bit", nullable: false),
                    PhysicalActivityRestrictionDuration = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PhysicalActivityRestrictionDurationArabic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActivityRestricted = table.Column<bool>(type: "bit", nullable: false),
                    RestrictedActivities = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RestrictedActivitiesArabic = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RequiresFollowUp = table.Column<bool>(type: "bit", nullable: false),
                    FollowUpDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FollowUpNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FollowUpNotesArabic = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PhysicianName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PhysicianNameArabic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PhysicianLicenseNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HospitalName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    HospitalNameArabic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsExternalReport = table.Column<bool>(type: "bit", nullable: false),
                    ExternalReportDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SentToTeachers = table.Column<bool>(type: "bit", nullable: false),
                    SentToTeachersAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IssuedByEmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IssuedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    NotesArabic = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalExcuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalExcuses_ClinicVisits_ClinicVisitId",
                        column: x => x.ClinicVisitId,
                        principalTable: "ClinicVisits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MedicalExcuses_Employees_IssuedByEmployeeId",
                        column: x => x.IssuedByEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MedicalExcuses_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MedicalExcuses_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentAllocations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentAllocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentAllocations_StudentInvoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "StudentInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentAllocations_StudentPayments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "StudentPayments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "AccountCode", "AccountId", "AccountNameAr", "AccountNameEn", "AccountType", "Balance", "CreatedAt", "CreatedBy", "DeletedAt", "Description", "IsActive", "IsDeleted", "Level", "ParentAccountId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), "1000", new Guid("10000000-0000-0000-0000-000000000001"), "الأصول", "Assets", 1, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", null, null, true, false, 1, null, null, null },
                    { new Guid("20000000-0000-0000-0000-000000000001"), "2000", new Guid("20000000-0000-0000-0000-000000000001"), "الالتزامات", "Liabilities", 2, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", null, null, true, false, 1, null, null, null },
                    { new Guid("30000000-0000-0000-0000-000000000001"), "3000", new Guid("30000000-0000-0000-0000-000000000001"), "حقوق الملكية", "Equity", 3, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", null, null, true, false, 1, null, null, null },
                    { new Guid("40000000-0000-0000-0000-000000000001"), "4000", new Guid("40000000-0000-0000-0000-000000000001"), "الإيرادات", "Revenue", 4, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", null, null, true, false, 1, null, null, null },
                    { new Guid("50000000-0000-0000-0000-000000000001"), "5000", new Guid("50000000-0000-0000-0000-000000000001"), "المصروفات", "Expenses", 5, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", null, null, true, false, 1, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "Tenants",
                columns: new[] { "Id", "Address", "City", "CreatedAt", "CreatedBy", "DeletedAt", "Email", "IsActive", "IsDeleted", "LicenseNumber", "LogoPath", "Name", "NameArabic", "Phone", "PrimaryColor", "SecondaryColor", "SubscriptionEndDate", "SubscriptionStartDate", "UpdatedAt", "UpdatedBy", "ZatcaCertificatePath", "ZatcaSecretKey" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000003"), null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", null, null, true, false, "MASAR-SYSTEM-001", null, "Masar Schools System", "نظام مَسَار للمدارس", null, null, null, new DateTime(2036, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "AccountCode", "AccountId", "AccountNameAr", "AccountNameEn", "AccountType", "Balance", "CreatedAt", "CreatedBy", "DeletedAt", "Description", "IsActive", "IsDeleted", "Level", "ParentAccountId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000002"), "1101", new Guid("10000000-0000-0000-0000-000000000002"), "الصندوق والخصوم النقدية", "Cash & Banks", 1, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", null, null, true, false, 2, new Guid("10000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("10000000-0000-0000-0000-000000000003"), "1102", new Guid("10000000-0000-0000-0000-000000000003"), "البنوك والمدفوعات الإلكترونية", "Banks & Electronic Payments", 1, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", null, null, true, false, 2, new Guid("10000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("10000000-0000-0000-0000-000000000004"), "1201", new Guid("10000000-0000-0000-0000-000000000004"), "مدينو الطلاب / ذمم أولياء الأمور", "Accounts Receivable - Students", 1, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", null, null, true, false, 2, new Guid("10000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("10000000-0000-0000-0000-000000000005"), "1301", new Guid("10000000-0000-0000-0000-000000000005"), "الأصول الثابتة", "Fixed Assets", 1, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", null, null, true, false, 2, new Guid("10000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("10000000-0000-0000-0000-000000000006"), "1302", new Guid("10000000-0000-0000-0000-000000000006"), "مجمع الإهلاك", "Accumulated Depreciation", 1, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", null, null, true, false, 2, new Guid("10000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("20000000-0000-0000-0000-000000000002"), "2101", new Guid("20000000-0000-0000-0000-000000000002"), "ضريبة القيمة المضافة المستحقة", "VAT Payable 15%", 2, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", null, null, true, false, 2, new Guid("20000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("20000000-0000-0000-0000-000000000003"), "2201", new Guid("20000000-0000-0000-0000-000000000003"), "الإيرادات غير المحققة / رسوم مقدماً", "Unearned Tuition Fees", 2, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", null, null, true, false, 2, new Guid("20000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("20000000-0000-0000-0000-000000000004"), "2301", new Guid("20000000-0000-0000-0000-000000000004"), "المرتبات المستحقة", "Payroll Payable", 2, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", null, null, true, false, 2, new Guid("20000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("20000000-0000-0000-0000-000000000005"), "2302", new Guid("20000000-0000-0000-0000-000000000005"), "التأمينات الاجتماعية المستحقة", "Social Security Payable", 2, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", null, null, true, false, 2, new Guid("20000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("30000000-0000-0000-0000-000000000002"), "3101", new Guid("30000000-0000-0000-0000-000000000002"), "رأس المال", "Capital", 3, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", null, null, true, false, 2, new Guid("30000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("30000000-0000-0000-0000-000000000003"), "3201", new Guid("30000000-0000-0000-0000-000000000003"), "الأرباح المحتجزة", "Retained Earnings", 3, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", null, null, true, false, 2, new Guid("30000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("40000000-0000-0000-0000-000000000002"), "4101", new Guid("40000000-0000-0000-0000-000000000002"), "إيرادات الرسوم الدراسية", "Tuition Revenue", 4, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", null, null, true, false, 2, new Guid("40000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("40000000-0000-0000-0000-000000000003"), "4102", new Guid("40000000-0000-0000-0000-000000000003"), "إيرادات النقل والمواصلات", "Transportation Revenue", 4, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", null, null, true, false, 2, new Guid("40000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("40000000-0000-0000-0000-000000000004"), "4103", new Guid("40000000-0000-0000-0000-000000000004"), "إيرادات الكتب والزي المدرسي", "Books & Uniforms Revenue", 4, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", null, null, true, false, 2, new Guid("40000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("40000000-0000-0000-0000-000000000005"), "4104", new Guid("40000000-0000-0000-0000-000000000005"), "إيرادات الأنشطة والاختبارات", "Activities Revenue", 4, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", null, null, true, false, 2, new Guid("40000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("40000000-0000-0000-0000-000000000006"), "4201", new Guid("40000000-0000-0000-0000-000000000006"), "الخصومات والمنح الممنوحة", "Discounts & Scholarships", 4, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", null, null, true, false, 2, new Guid("40000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("50000000-0000-0000-0000-000000000002"), "5101", new Guid("50000000-0000-0000-0000-000000000002"), "المصروفات الإدارية", "Administrative Expenses", 5, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", null, null, true, false, 2, new Guid("50000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("50000000-0000-0000-0000-000000000003"), "5102", new Guid("50000000-0000-0000-0000-000000000003"), "المصروفات التشغيلية", "Operating Expenses", 5, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", null, null, true, false, 2, new Guid("50000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("50000000-0000-0000-0000-000000000004"), "5103", new Guid("50000000-0000-0000-0000-000000000004"), "مصروفات الديون المعدومة", "Bad Debt Expenses", 5, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", null, null, true, false, 2, new Guid("50000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("50000000-0000-0000-0000-000000000005"), "5201", new Guid("50000000-0000-0000-0000-000000000005"), "مرتبات الموظفين", "Salaries & Wages", 5, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", null, null, true, false, 2, new Guid("50000000-0000-0000-0000-000000000001"), null, null }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "DescriptionArabic", "IsActive", "Name", "NormalizedName", "TenantId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000002"), "INITIAL_SEED", "System-wide administrator with full access", "مشرف النظام بصلاحيات كاملة", true, "Super Admin", "SUPER ADMIN", new Guid("00000000-0000-0000-0000-000000000003") });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "BranchId", "ConcurrencyStamp", "Email", "EmailConfirmed", "FailedLoginAttempts", "FirstName", "FirstNameArabic", "FullName", "FullNameArabic", "IsActive", "IsSuperAdmin", "LastLoginAt", "LastName", "LastNameArabic", "LockoutEnabled", "LockoutEnd", "NationalId", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfileImagePath", "SchoolId", "SecurityStamp", "TenantId", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), 0, null, "INITIAL_CONCURRENCY_STAMP", "superadmin@masar.sa", false, 0, "Super", "مشرف", "Super Admin", "المشرف العام", true, true, null, "Admin", "عام", false, null, null, "SUPERADMIN@MASAR.SA", "SUPERADMIN", "AQAAAAIAAYagAAAAEAz7yeP4lkZ0E5qgnAXGXtIwsK3YN6MG7vfmIfmVt1IF5+n8+WTWo3Tbdr1EfVbjNg==", null, false, null, null, "INITIAL_SECURITY_STAMP", new Guid("00000000-0000-0000-0000-000000000003"), false, "superadmin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000001") });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicRecords_AcademicYear",
                table: "AcademicRecords",
                column: "AcademicYear");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicRecords_StudentId",
                table: "AcademicRecords",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicRecords_StudentId_AcademicYear",
                table: "AcademicRecords",
                columns: new[] { "StudentId", "AcademicYear" });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicTerms_SchoolId",
                table: "AcademicTerms",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountCode",
                table: "Accounts",
                column: "AccountCode",
                unique: true,
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_ParentAccountId",
                table: "Accounts",
                column: "ParentAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionApplications_ConvertedStudentId",
                table: "AdmissionApplications",
                column: "ConvertedStudentId",
                unique: true,
                filter: "[ConvertedStudentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionApplications_GradeLevelId",
                table: "AdmissionApplications",
                column: "GradeLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionApplications_ReviewedByEmployeeId",
                table: "AdmissionApplications",
                column: "ReviewedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionApplications_SchoolId",
                table: "AdmissionApplications",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionApplications_SectionId",
                table: "AdmissionApplications",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionApplications_SubmittedByEmployeeId",
                table: "AdmissionApplications",
                column: "SubmittedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionExams_AdmissionApplicationId",
                table: "AdmissionExams",
                column: "AdmissionApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionExams_EvaluatedByEmployeeId",
                table: "AdmissionExams",
                column: "EvaluatedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionExams_ExaminerId",
                table: "AdmissionExams",
                column: "ExaminerId");

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionExams_InterviewerId",
                table: "AdmissionExams",
                column: "InterviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionExams_SchoolId",
                table: "AdmissionExams",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_AiChatHistories_CreatedAt",
                table: "AiChatHistories",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AiChatHistories_TenantId",
                table: "AiChatHistories",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AiChatHistories_UserId",
                table: "AiChatHistories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AlumniRecords_CreatedByEmployeeId",
                table: "AlumniRecords",
                column: "CreatedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AlumniRecords_SchoolId",
                table: "AlumniRecords",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_AlumniRecords_StudentId",
                table: "AlumniRecords",
                column: "StudentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationDocuments_AdmissionApplicationId",
                table: "ApplicationDocuments",
                column: "AdmissionApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationDocuments_UploadedByEmployeeId",
                table: "ApplicationDocuments",
                column: "UploadedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationDocuments_VerifiedByEmployeeId",
                table: "ApplicationDocuments",
                column: "VerifiedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_TenantId",
                table: "AspNetRoles",
                column: "TenantId");

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
                name: "IX_AspNetUsers_BranchId",
                table: "AspNetUsers",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers",
                column: "Email",
                unique: true,
                filter: "IsActive = 1");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_NationalId",
                table: "AspNetUsers",
                column: "NationalId",
                filter: "IsActive = 1");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SchoolId",
                table: "AspNetUsers",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TenantId",
                table: "AspNetUsers",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserName",
                table: "AspNetUsers",
                column: "UserName",
                unique: true,
                filter: "IsActive = 1");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceLogs_BiometricId_EventAt_Direction_Source",
                table: "AttendanceLogs",
                columns: new[] { "BiometricId", "EventAt", "Direction", "Source" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceLogs_EmployeeId",
                table: "AttendanceLogs",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceLogs_SchoolId",
                table: "AttendanceLogs",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendancePermits_EmployeeId",
                table: "AttendancePermits",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendancePermits_SchoolId",
                table: "AttendancePermits",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_ClassRoomId",
                table: "AttendanceRecords",
                column: "ClassRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_EmployeeId",
                table: "AttendanceRecords",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_StudentId",
                table: "AttendanceRecords",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_FinancialPeriodId",
                table: "AuditLogs",
                column: "FinancialPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_FinancialPeriodId1",
                table: "AuditLogs",
                column: "FinancialPeriodId1");

            migrationBuilder.CreateIndex(
                name: "IX_BackupRecords_TenantId",
                table: "BackupRecords",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_BackupSettings_TenantId",
                table: "BackupSettings",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_SchoolId",
                table: "Branches",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_BusAttendances_AttendanceDate",
                table: "BusAttendances",
                column: "AttendanceDate");

            migrationBuilder.CreateIndex(
                name: "IX_BusAttendances_BusId",
                table: "BusAttendances",
                column: "BusId");

            migrationBuilder.CreateIndex(
                name: "IX_BusAttendances_BusScheduleId",
                table: "BusAttendances",
                column: "BusScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_BusAttendances_StopId",
                table: "BusAttendances",
                column: "StopId");

            migrationBuilder.CreateIndex(
                name: "IX_BusAttendances_StudentId",
                table: "BusAttendances",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_BusAttendances_TenantId1",
                table: "BusAttendances",
                column: "TenantId1");

            migrationBuilder.CreateIndex(
                name: "IX_Buses_BusNumber",
                table: "Buses",
                column: "BusNumber",
                unique: true,
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Buses_PlateNumber",
                table: "Buses",
                column: "PlateNumber",
                unique: true,
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Buses_TenantId1",
                table: "Buses",
                column: "TenantId1");

            migrationBuilder.CreateIndex(
                name: "IX_BusIncidents_BusId",
                table: "BusIncidents",
                column: "BusId");

            migrationBuilder.CreateIndex(
                name: "IX_BusIncidents_IncidentDate",
                table: "BusIncidents",
                column: "IncidentDate");

            migrationBuilder.CreateIndex(
                name: "IX_BusIncidents_TenantId1",
                table: "BusIncidents",
                column: "TenantId1");

            migrationBuilder.CreateIndex(
                name: "IX_BusRoutes_BusId",
                table: "BusRoutes",
                column: "BusId");

            migrationBuilder.CreateIndex(
                name: "IX_BusRoutes_RouteCode",
                table: "BusRoutes",
                column: "RouteCode",
                unique: true,
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_BusRoutes_TenantId1",
                table: "BusRoutes",
                column: "TenantId1");

            migrationBuilder.CreateIndex(
                name: "IX_BusSchedules_BusId",
                table: "BusSchedules",
                column: "BusId");

            migrationBuilder.CreateIndex(
                name: "IX_BusSchedules_DepartureTime",
                table: "BusSchedules",
                column: "DepartureTime");

            migrationBuilder.CreateIndex(
                name: "IX_BusSchedules_DriverId",
                table: "BusSchedules",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_BusSchedules_RouteId",
                table: "BusSchedules",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_BusSchedules_SchoolId",
                table: "BusSchedules",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_BusSchedules_SupervisorId",
                table: "BusSchedules",
                column: "SupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_BusSchedules_TenantId1",
                table: "BusSchedules",
                column: "TenantId1");

            migrationBuilder.CreateIndex(
                name: "IX_BusSupervisors_AssignedBusId",
                table: "BusSupervisors",
                column: "AssignedBusId");

            migrationBuilder.CreateIndex(
                name: "IX_BusSupervisors_EmployeeId",
                table: "BusSupervisors",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_BusSupervisors_TenantId1",
                table: "BusSupervisors",
                column: "TenantId1");

            migrationBuilder.CreateIndex(
                name: "IX_BusTrackingLogs_BusId",
                table: "BusTrackingLogs",
                column: "BusId");

            migrationBuilder.CreateIndex(
                name: "IX_BusTrackingLogs_Timestamp",
                table: "BusTrackingLogs",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_CanteenOrderItemExtras_CanteenProductExtraId",
                table: "CanteenOrderItemExtras",
                column: "CanteenProductExtraId");

            migrationBuilder.CreateIndex(
                name: "IX_CanteenOrderItems_CanteenOrderId",
                table: "CanteenOrderItems",
                column: "CanteenOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_CanteenOrderItems_CanteenProductId",
                table: "CanteenOrderItems",
                column: "CanteenProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CanteenOrderItems_VariantId",
                table: "CanteenOrderItems",
                column: "VariantId");

            migrationBuilder.CreateIndex(
                name: "IX_CanteenOrders_BranchId",
                table: "CanteenOrders",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_CanteenOrders_CanteenTableId",
                table: "CanteenOrders",
                column: "CanteenTableId");

            migrationBuilder.CreateIndex(
                name: "IX_CanteenOrders_CashierShiftId",
                table: "CanteenOrders",
                column: "CashierShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_CanteenOrders_DeliveryEmployeeId",
                table: "CanteenOrders",
                column: "DeliveryEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_CanteenOrders_DeliveryZoneId",
                table: "CanteenOrders",
                column: "DeliveryZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_CanteenOrders_PickUpPointId",
                table: "CanteenOrders",
                column: "PickUpPointId");

            migrationBuilder.CreateIndex(
                name: "IX_CanteenOrders_SchoolId",
                table: "CanteenOrders",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_CanteenOrders_StudentId",
                table: "CanteenOrders",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_CanteenPayments_CanteenOrderId",
                table: "CanteenPayments",
                column: "CanteenOrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CanteenProductExtras_SchoolId",
                table: "CanteenProductExtras",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_CanteenProductExtrasMapping_ProductsId",
                table: "CanteenProductExtrasMapping",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_CanteenProductImages_CanteenProductId",
                table: "CanteenProductImages",
                column: "CanteenProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CanteenProducts_ProductCategoryId",
                table: "CanteenProducts",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CanteenProducts_SchoolId",
                table: "CanteenProducts",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_CanteenProductVariants_CanteenProductId",
                table: "CanteenProductVariants",
                column: "CanteenProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CanteenTables_BranchId",
                table: "CanteenTables",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_CanteenTables_SchoolId",
                table: "CanteenTables",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_CashierShifts_BranchId",
                table: "CashierShifts",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_CashierShifts_EmployeeId",
                table: "CashierShifts",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_CashierShifts_SchoolId",
                table: "CashierShifts",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ChatRoomId",
                table: "ChatMessages",
                column: "ChatRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ReceiverId",
                table: "ChatMessages",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ReplyToMessageId",
                table: "ChatMessages",
                column: "ReplyToMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_SenderId",
                table: "ChatMessages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatRoomMembers_ChatRoomId",
                table: "ChatRoomMembers",
                column: "ChatRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatRoomMembers_UserId",
                table: "ChatRoomMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatRooms_ClassRoomId",
                table: "ChatRooms",
                column: "ClassRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatRooms_SchoolId",
                table: "ChatRooms",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatRooms_TenantId",
                table: "ChatRooms",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassRooms_BranchId",
                table: "ClassRooms",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassRooms_ClassTeacherId",
                table: "ClassRooms",
                column: "ClassTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassRooms_GradeLevelId",
                table: "ClassRooms",
                column: "GradeLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Classrooms_School_GradeLevel",
                table: "ClassRooms",
                columns: new[] { "SchoolId", "GradeLevelLegacy", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_ClassRooms_SectionId",
                table: "ClassRooms",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassRooms_TenantId",
                table: "ClassRooms",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "UX_Classrooms_School_Code",
                table: "ClassRooms",
                columns: new[] { "SchoolId", "Code" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicVisits_AttendingEmployeeId",
                table: "ClinicVisits",
                column: "AttendingEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicVisits_HealthProfileId",
                table: "ClinicVisits",
                column: "HealthProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicVisits_SchoolId",
                table: "ClinicVisits",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicVisits_StudentId",
                table: "ClinicVisits",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractArchives_EmployeeId",
                table: "ContractArchives",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractArchives_SchoolId",
                table: "ContractArchives",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_EmployeeId_ContractNumber",
                table: "Contracts",
                columns: new[] { "EmployeeId", "ContractNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_SchoolId",
                table: "Contracts",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryZones_BranchId",
                table: "DeliveryZones",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryZones_SchoolId",
                table: "DeliveryZones",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_DiscountCode",
                table: "Discounts",
                column: "DiscountCode",
                unique: true,
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentActivityLogs_DocumentId",
                table: "DocumentActivityLogs",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentActivityLogs_StudentId",
                table: "DocumentActivityLogs",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_AssignedBusId",
                table: "Drivers",
                column: "AssignedBusId");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_EmployeeId",
                table: "Drivers",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_LicenseNumber",
                table: "Drivers",
                column: "LicenseNumber",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_TenantId1",
                table: "Drivers",
                column: "TenantId1");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_BranchId",
                table: "Employees",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_MasarUserId",
                table: "Employees",
                column: "MasarUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_SchoolId",
                table: "Employees",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialPeriods_SchoolId",
                table: "FinancialPeriods",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialPeriods_TenantId",
                table: "FinancialPeriods",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelRecords_BusId",
                table: "FuelRecords",
                column: "BusId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelRecords_RefuelDate",
                table: "FuelRecords",
                column: "RefuelDate");

            migrationBuilder.CreateIndex(
                name: "IX_FuelRecords_TenantId1",
                table: "FuelRecords",
                column: "TenantId1");

            migrationBuilder.CreateIndex(
                name: "IX_GradeLevels_Code",
                table: "GradeLevels",
                column: "Code",
                unique: true,
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_StudentId",
                table: "Grades",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_TeacherId",
                table: "Grades",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_GraduationClearanceRecords_CreatedByEmployeeId",
                table: "GraduationClearanceRecords",
                column: "CreatedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_GraduationClearanceRecords_StudentId",
                table: "GraduationClearanceRecords",
                column: "StudentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GraduationDocuments_AlumniRecordId",
                table: "GraduationDocuments",
                column: "AlumniRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_GraduationDocuments_CreatedByEmployeeId",
                table: "GraduationDocuments",
                column: "CreatedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Guardians_TenantId",
                table: "Guardians",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_PreferredSupplierId",
                table: "InventoryItems",
                column: "PreferredSupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_SupplierId",
                table: "InventoryItems",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransferItems_InventoryItemId",
                table: "InventoryTransferItems",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransferItems_InventoryTransferId",
                table: "InventoryTransferItems",
                column: "InventoryTransferId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransfers_FromWarehouseId",
                table: "InventoryTransfers",
                column: "FromWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransfers_ToWarehouseId",
                table: "InventoryTransfers",
                column: "ToWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_InvoiceId",
                table: "InvoiceItems",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceLineItems_InvoiceId",
                table: "InvoiceLineItems",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoicePayments_InvoiceId",
                table: "InvoicePayments",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_InvoiceNumber",
                table: "Invoices",
                column: "InvoiceNumber",
                unique: true,
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_StudentId",
                table: "Invoices",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_TenantId",
                table: "Invoices",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_ZatcaInvoiceUuid",
                table: "Invoices",
                column: "ZatcaInvoiceUuid",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_EntryDate",
                table: "JournalEntries",
                column: "EntryDate",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_EntryNumber",
                table: "JournalEntries",
                column: "EntryNumber",
                unique: true,
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryLines_AccountId",
                table: "JournalEntryLines",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryLines_AccountId1",
                table: "JournalEntryLines",
                column: "AccountId1");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryLines_EntryId",
                table: "JournalEntryLines",
                column: "EntryId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryLines_StudentId",
                table: "JournalEntryLines",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveBalances_EmployeeId",
                table: "LeaveBalances",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveBalances_SchoolId",
                table: "LeaveBalances",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_EmployeeId",
                table: "LeaveRequests",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_SchoolId",
                table: "LeaveRequests",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveTypes_SchoolId",
                table: "LeaveTypes",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRecords_BusId",
                table: "MaintenanceRecords",
                column: "BusId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRecords_MaintenanceDate",
                table: "MaintenanceRecords",
                column: "MaintenanceDate");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRecords_TenantId1",
                table: "MaintenanceRecords",
                column: "TenantId1");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalExcuses_ClinicVisitId",
                table: "MedicalExcuses",
                column: "ClinicVisitId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalExcuses_IssuedByEmployeeId",
                table: "MedicalExcuses",
                column: "IssuedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalExcuses_SchoolId",
                table: "MedicalExcuses",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalExcuses_StudentId",
                table: "MedicalExcuses",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalItems_CreatedByEmployeeId",
                table: "MedicalItems",
                column: "CreatedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalItems_SchoolId",
                table: "MedicalItems",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalItems_UpdatedByEmployeeId",
                table: "MedicalItems",
                column: "UpdatedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Nationalities_SchoolId",
                table: "Nationalities",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentAllocations_InvoiceId",
                table: "PaymentAllocations",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentAllocations_PaymentId",
                table: "PaymentAllocations",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollArchives_EmployeeId",
                table: "PayrollArchives",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollArchives_SchoolId",
                table: "PayrollArchives",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollProfiles_EmployeeId",
                table: "PayrollProfiles",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollProfiles_SchoolId",
                table: "PayrollProfiles",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_MasarRoleId",
                table: "Permissions",
                column: "MasarRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_PickUpPoints_DeliveryZoneId",
                table: "PickUpPoints",
                column: "DeliveryZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_PickUpPoints_SchoolId",
                table: "PickUpPoints",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_SchoolId",
                table: "ProductCategories",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoiceItems_InventoryItemId",
                table: "PurchaseInvoiceItems",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoiceItems_PurchaseInvoiceId",
                table: "PurchaseInvoiceItems",
                column: "PurchaseInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoices_PurchaseOrderId",
                table: "PurchaseInvoices",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoices_SchoolId",
                table: "PurchaseInvoices",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoices_SupplierId",
                table: "PurchaseInvoices",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderItems_InventoryItemId",
                table: "PurchaseOrderItems",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderItems_PurchaseOrderId",
                table: "PurchaseOrderItems",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_SupplierId",
                table: "PurchaseOrders",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_WarehouseId",
                table: "PurchaseOrders",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequestItems_InventoryItemId",
                table: "PurchaseRequestItems",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequestItems_PurchaseRequestId",
                table: "PurchaseRequestItems",
                column: "PurchaseRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_SchoolId",
                table: "PurchaseRequests",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ResignedEmployees_EmployeeId",
                table: "ResignedEmployees",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ResignedEmployees_SchoolId",
                table: "ResignedEmployees",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskAlertHistories_AlertType",
                table: "RiskAlertHistories",
                column: "AlertType");

            migrationBuilder.CreateIndex(
                name: "IX_RiskAlertHistories_Status",
                table: "RiskAlertHistories",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_RiskAlertHistories_StudentId",
                table: "RiskAlertHistories",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleId",
                table: "RolePermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RollCallRecords_SessionId",
                table: "RollCallRecords",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_RollCallRecords_Status",
                table: "RollCallRecords",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_RollCallRecords_StudentId",
                table: "RollCallRecords",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_RollCallSessions_ClassRoomId",
                table: "RollCallSessions",
                column: "ClassRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RollCallSessions_ClassRoomId_SessionDate",
                table: "RollCallSessions",
                columns: new[] { "ClassRoomId", "SessionDate" });

            migrationBuilder.CreateIndex(
                name: "IX_RollCallSessions_SessionDate",
                table: "RollCallSessions",
                column: "SessionDate");

            migrationBuilder.CreateIndex(
                name: "IX_RollCallSessions_TeacherId",
                table: "RollCallSessions",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteStops_RouteId",
                table: "RouteStops",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteStops_TenantId1",
                table: "RouteStops",
                column: "TenantId1");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_NoorSchoolId",
                table: "Schools",
                column: "NoorSchoolId",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_TenantId",
                table: "Schools",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_AcademicTermId",
                table: "Sections",
                column: "AcademicTermId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_Code",
                table: "Sections",
                column: "Code",
                unique: true,
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustmentItems_InventoryItemId",
                table: "StockAdjustmentItems",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustmentItems_StockAdjustmentId",
                table: "StockAdjustmentItems",
                column: "StockAdjustmentId");

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustments_WarehouseId",
                table: "StockAdjustments",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustments_WarehouseId1",
                table: "StockAdjustments",
                column: "WarehouseId1");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_InventoryItemId",
                table: "StockTransactions",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_InventoryItemId1",
                table: "StockTransactions",
                column: "InventoryItemId1");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_WarehouseId",
                table: "StockTransactions",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_WarehouseId1",
                table: "StockTransactions",
                column: "WarehouseId1");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAccounts_AccountNumber",
                table: "StudentAccounts",
                column: "AccountNumber",
                unique: true,
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAccounts_StudentId",
                table: "StudentAccounts",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAccounts_StudentId1",
                table: "StudentAccounts",
                column: "StudentId1",
                unique: true,
                filter: "[StudentId1] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_StudentDiscounts_DiscountId",
                table: "StudentDiscounts",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentDiscounts_InvoiceId",
                table: "StudentDiscounts",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentDiscounts_StudentAccountId",
                table: "StudentDiscounts",
                column: "StudentAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentDocuments_Category",
                table: "StudentDocuments",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_StudentDocuments_DocumentType",
                table: "StudentDocuments",
                column: "DocumentType");

            migrationBuilder.CreateIndex(
                name: "IX_StudentDocuments_ExpiryDate",
                table: "StudentDocuments",
                column: "ExpiryDate");

            migrationBuilder.CreateIndex(
                name: "IX_StudentDocuments_PreviousDocumentId",
                table: "StudentDocuments",
                column: "PreviousDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentDocuments_StudentId",
                table: "StudentDocuments",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentHealthProfiles_CreatedByEmployeeId",
                table: "StudentHealthProfiles",
                column: "CreatedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentHealthProfiles_StudentId",
                table: "StudentHealthProfiles",
                column: "StudentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentInvoices_InvoiceNumber",
                table: "StudentInvoices",
                column: "InvoiceNumber",
                unique: true,
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_StudentInvoices_StudentAccountId",
                table: "StudentInvoices",
                column: "StudentAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentInvoices_UUID",
                table: "StudentInvoices",
                column: "UUID",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_StudentPayments_InvoiceId",
                table: "StudentPayments",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentPayments_PaymentNumber",
                table: "StudentPayments",
                column: "PaymentNumber",
                unique: true,
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_StudentPayments_StudentAccountId",
                table: "StudentPayments",
                column: "StudentAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentRiskScores_CalculatedAt",
                table: "StudentRiskScores",
                column: "CalculatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_StudentRiskScores_RiskLevel",
                table: "StudentRiskScores",
                column: "RiskLevel");

            migrationBuilder.CreateIndex(
                name: "IX_StudentRiskScores_StudentId",
                table: "StudentRiskScores",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentRiskScores_StudentId_CalculatedAt",
                table: "StudentRiskScores",
                columns: new[] { "StudentId", "CalculatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Students_BranchId",
                table: "Students",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_ClassRoomId",
                table: "Students",
                column: "ClassRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_GuardianId",
                table: "Students",
                column: "GuardianId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_NationalId",
                table: "Students",
                column: "NationalId",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Students_SchoolId",
                table: "Students",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentNumber",
                table: "Students",
                column: "StudentNumber",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTransportSubscriptions_BusRouteId",
                table: "StudentTransportSubscriptions",
                column: "BusRouteId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTransportSubscriptions_BusScheduleId",
                table: "StudentTransportSubscriptions",
                column: "BusScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTransportSubscriptions_RouteStopId",
                table: "StudentTransportSubscriptions",
                column: "RouteStopId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTransportSubscriptions_StudentId",
                table: "StudentTransportSubscriptions",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTransportSubscriptions_TenantId1",
                table: "StudentTransportSubscriptions",
                column: "TenantId1");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_LicenseNumber",
                table: "Tenants",
                column: "LicenseNumber",
                unique: true,
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_TermArchives_SchoolId",
                table: "TermArchives",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_TermArchives_TermId",
                table: "TermArchives",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_PermissionId",
                table: "UserPermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_UserId",
                table: "UserPermissions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_BranchId",
                table: "Warehouses",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_SchoolId",
                table: "Warehouses",
                column: "SchoolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcademicRecords");

            migrationBuilder.DropTable(
                name: "AdmissionExams");

            migrationBuilder.DropTable(
                name: "AiChatHistories");

            migrationBuilder.DropTable(
                name: "ApplicationDocuments");

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
                name: "AttendanceLogs");

            migrationBuilder.DropTable(
                name: "AttendancePermits");

            migrationBuilder.DropTable(
                name: "AttendanceRecords");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "BackupRecords");

            migrationBuilder.DropTable(
                name: "BackupSettings");

            migrationBuilder.DropTable(
                name: "BusAttendances");

            migrationBuilder.DropTable(
                name: "BusIncidents");

            migrationBuilder.DropTable(
                name: "BusTrackingLogs");

            migrationBuilder.DropTable(
                name: "CanteenOrderItemExtras");

            migrationBuilder.DropTable(
                name: "CanteenPayments");

            migrationBuilder.DropTable(
                name: "CanteenProductExtrasMapping");

            migrationBuilder.DropTable(
                name: "CanteenProductImages");

            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "ChatRoomMembers");

            migrationBuilder.DropTable(
                name: "ContractArchives");

            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "DocumentActivityLogs");

            migrationBuilder.DropTable(
                name: "FuelRecords");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "GraduationClearanceRecords");

            migrationBuilder.DropTable(
                name: "GraduationDocuments");

            migrationBuilder.DropTable(
                name: "InventoryTransferItems");

            migrationBuilder.DropTable(
                name: "InvoiceItems");

            migrationBuilder.DropTable(
                name: "InvoiceLineItems");

            migrationBuilder.DropTable(
                name: "InvoicePayments");

            migrationBuilder.DropTable(
                name: "JournalEntryLines");

            migrationBuilder.DropTable(
                name: "LeaveBalances");

            migrationBuilder.DropTable(
                name: "LeaveRequests");

            migrationBuilder.DropTable(
                name: "LeaveTypes");

            migrationBuilder.DropTable(
                name: "MaintenanceRecords");

            migrationBuilder.DropTable(
                name: "MedicalExcuses");

            migrationBuilder.DropTable(
                name: "MedicalItems");

            migrationBuilder.DropTable(
                name: "Nationalities");

            migrationBuilder.DropTable(
                name: "PaymentAllocations");

            migrationBuilder.DropTable(
                name: "PayrollArchives");

            migrationBuilder.DropTable(
                name: "PayrollProfiles");

            migrationBuilder.DropTable(
                name: "PurchaseInvoiceItems");

            migrationBuilder.DropTable(
                name: "PurchaseOrderItems");

            migrationBuilder.DropTable(
                name: "PurchaseRequestItems");

            migrationBuilder.DropTable(
                name: "ResignedEmployees");

            migrationBuilder.DropTable(
                name: "RiskAlertHistories");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "RollCallRecords");

            migrationBuilder.DropTable(
                name: "StockAdjustmentItems");

            migrationBuilder.DropTable(
                name: "StockTransactions");

            migrationBuilder.DropTable(
                name: "StudentDiscounts");

            migrationBuilder.DropTable(
                name: "StudentRiskScores");

            migrationBuilder.DropTable(
                name: "StudentTransportSubscriptions");

            migrationBuilder.DropTable(
                name: "TermArchives");

            migrationBuilder.DropTable(
                name: "UserPermissions");

            migrationBuilder.DropTable(
                name: "AdmissionApplications");

            migrationBuilder.DropTable(
                name: "FinancialPeriods");

            migrationBuilder.DropTable(
                name: "CanteenOrderItems");

            migrationBuilder.DropTable(
                name: "CanteenProductExtras");

            migrationBuilder.DropTable(
                name: "ChatRooms");

            migrationBuilder.DropTable(
                name: "StudentDocuments");

            migrationBuilder.DropTable(
                name: "AlumniRecords");

            migrationBuilder.DropTable(
                name: "InventoryTransfers");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "JournalEntries");

            migrationBuilder.DropTable(
                name: "ClinicVisits");

            migrationBuilder.DropTable(
                name: "StudentPayments");

            migrationBuilder.DropTable(
                name: "PurchaseInvoices");

            migrationBuilder.DropTable(
                name: "PurchaseRequests");

            migrationBuilder.DropTable(
                name: "RollCallSessions");

            migrationBuilder.DropTable(
                name: "StockAdjustments");

            migrationBuilder.DropTable(
                name: "InventoryItems");

            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "BusSchedules");

            migrationBuilder.DropTable(
                name: "RouteStops");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "CanteenOrders");

            migrationBuilder.DropTable(
                name: "CanteenProductVariants");

            migrationBuilder.DropTable(
                name: "StudentHealthProfiles");

            migrationBuilder.DropTable(
                name: "StudentInvoices");

            migrationBuilder.DropTable(
                name: "PurchaseOrders");

            migrationBuilder.DropTable(
                name: "BusSupervisors");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "BusRoutes");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "CanteenTables");

            migrationBuilder.DropTable(
                name: "CashierShifts");

            migrationBuilder.DropTable(
                name: "PickUpPoints");

            migrationBuilder.DropTable(
                name: "CanteenProducts");

            migrationBuilder.DropTable(
                name: "StudentAccounts");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Warehouses");

            migrationBuilder.DropTable(
                name: "Buses");

            migrationBuilder.DropTable(
                name: "DeliveryZones");

            migrationBuilder.DropTable(
                name: "ProductCategories");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "ClassRooms");

            migrationBuilder.DropTable(
                name: "Guardians");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "GradeLevels");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "AcademicTerms");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "Schools");

            migrationBuilder.DropTable(
                name: "Tenants");
        }
    }
}
