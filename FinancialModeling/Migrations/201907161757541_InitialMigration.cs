namespace FinancialModeling.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApiErrors",
                c => new
                    {
                        ApiErrorId = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        RequestMethod = c.String(),
                        RequestUri = c.String(),
                        TimeUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ApiErrorId);
            
            CreateTable(
                "dbo.ClientModels",
                c => new
                    {
                        ClientModelId = c.Int(nullable: false, identity: true),
                        ClientId = c.Int(nullable: false),
                        ParkingTypeId = c.Int(nullable: false),
                        ModelTypeId = c.Int(nullable: false),
                        IsSetupDone = c.Boolean(nullable: false),
                        CreatedById = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdatedById = c.String(),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ClientModelId);
            
            CreateTable(
                "dbo.EquipmentCosts",
                c => new
                    {
                        EquipmentId = c.Int(nullable: false, identity: true),
                        UnitsOwned = c.Int(nullable: false),
                        UnitsPurchased = c.Int(nullable: false),
                        CostOfBaseUnit = c.Int(nullable: false),
                        WarrantyStartingYear = c.Int(nullable: false),
                        MonthlyMeterSoftwareFees = c.Int(nullable: false),
                        MonthlyCreditCardProcessingFees = c.Int(nullable: false),
                        EstimatedCreditCardTransaction = c.Int(nullable: false),
                        EquipmentTypeId = c.Int(nullable: false),
                        ClientId = c.Int(nullable: false),
                        ZoneCode = c.Int(nullable: false),
                        ParkingTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EquipmentId);
            
            CreateTable(
                "dbo.EscalatingOperatingHours",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClientId = c.Int(nullable: false),
                        ZoneId = c.Int(nullable: false),
                        ClientModelId = c.Int(nullable: false),
                        StartTime = c.String(),
                        EndTime = c.String(),
                        TotalHours = c.Int(nullable: false),
                        OperatingHourType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EscalatingZones",
                c => new
                    {
                        ZoneId = c.Int(nullable: false, identity: true),
                        ClientModelId = c.Int(nullable: false),
                        ClientId = c.Int(nullable: false),
                        NonPeakHourlyRate = c.Int(nullable: false),
                        NonPeakEscalatingRate = c.Int(nullable: false),
                        NonPeakHourEscalatingRateBegins = c.Int(nullable: false),
                        NonPeakDailyMaxOrAllDayRate = c.Int(nullable: false),
                        NonPeakEveningFlatRate = c.Int(nullable: false),
                        PeakHourlyRate = c.Int(nullable: false),
                        PeakEscalatingRate = c.Int(nullable: false),
                        PeakHourEscalatingRateBegins = c.Int(nullable: false),
                        PeakDailyMaxOrAllDayRate = c.Int(nullable: false),
                        PeakEveningFlatRate = c.Int(nullable: false),
                        NumberOfSpacesPerZone = c.Int(nullable: false),
                        PercentOfSpaceOccupied = c.Int(nullable: false),
                        NumberOfSpacesRemaining = c.Int(nullable: false),
                        CompliancePercentage = c.Int(nullable: false),
                        NonPeakOccupancyPercentage = c.Int(nullable: false),
                        PeakOccupancyPercentage = c.Int(nullable: false),
                        ZoneCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ZoneId);
            
            CreateTable(
                "dbo.HourlyOperatingHours",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClientId = c.Int(nullable: false),
                        ZoneId = c.Int(nullable: false),
                        StartTime = c.String(),
                        EndTime = c.String(),
                        TotalHours = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HourlyZones",
                c => new
                    {
                        ZoneId = c.Int(nullable: false, identity: true),
                        ClientModelId = c.Int(nullable: false),
                        ClientId = c.Int(nullable: false),
                        NonPeakSeasonHourlyRate = c.Int(nullable: false),
                        PeakSeasonHourlyRate = c.Int(nullable: false),
                        NumberOfSpacesPerZone = c.Int(nullable: false),
                        PercentOfSpaceOccupied = c.Int(nullable: false),
                        NumberOfSpacesRemaining = c.Int(nullable: false),
                        CompliancePercentage = c.Int(nullable: false),
                        NonPeakOccupancyPercentage = c.Int(nullable: false),
                        PeakOccupancyPercentage = c.Int(nullable: false),
                        ZoneCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ZoneId);
            
            CreateTable(
                "dbo.LuEquipmentTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        TypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LuModelTypes",
                c => new
                    {
                        ModelTypeId = c.Int(nullable: false, identity: true),
                        ModelTypeName = c.String(),
                    })
                .PrimaryKey(t => t.ModelTypeId);
            
            CreateTable(
                "dbo.LuParkingTypes",
                c => new
                    {
                        ParkingTypeId = c.Int(nullable: false, identity: true),
                        ParkingTypeName = c.String(),
                    })
                .PrimaryKey(t => t.ParkingTypeId);
            
            CreateTable(
                "dbo.OperatingDays",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DaysPerYear = c.Int(nullable: false),
                        PeakDays = c.Int(nullable: false),
                        OffDays = c.Int(nullable: false),
                        NonPeakDays = c.Int(nullable: false),
                        ClientId = c.Int(nullable: false),
                        ClientModelId = c.Int(nullable: false),
                        ZoneId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ParkingClients",
                c => new
                    {
                        ClientId = c.Int(nullable: false, identity: true),
                        ClientName = c.String(),
                        OnStreetZoneCount = c.Int(nullable: false),
                        OffStreetZoneCount = c.Int(nullable: false),
                        GaragesZoneCount = c.Int(nullable: false),
                        OnStreetPermitCount = c.Int(nullable: false),
                        OffStreetPermitCount = c.Int(nullable: false),
                        GaragesPermitCount = c.Int(nullable: false),
                        CreatedById = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdatedById = c.String(),
                        UpdatedDate = c.DateTime(),
                        NumberOfUsers = c.Int(nullable: false),
                        IsPeakSeasonPricing = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ClientId);
            
            CreateTable(
                "dbo.PermitDetails",
                c => new
                    {
                        PermitId = c.Int(nullable: false, identity: true),
                        AnnualCost = c.Int(nullable: false),
                        QuantitySold = c.Int(nullable: false),
                        PermitCode = c.Int(nullable: false),
                        ClientId = c.Int(nullable: false),
                        ClientModelId = c.Int(nullable: false),
                        ZoneId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PermitId);
            
            CreateTable(
                "dbo.Permits",
                c => new
                    {
                        PermitCode = c.Int(nullable: false, identity: true),
                        PermitName = c.String(),
                        ClientId = c.Int(nullable: false),
                        ZoneId = c.Int(nullable: false),
                        ParkingTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PermitCode);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.TimeOfDayOperatingHours",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClientId = c.Int(nullable: false),
                        ZoneId = c.Int(nullable: false),
                        ClientModelId = c.Int(nullable: false),
                        PeakSeasonHourlyRate = c.Int(nullable: false),
                        NonPeakSeasonHourlyRate = c.Int(nullable: false),
                        OperatingHoursStart = c.String(),
                        OperatingHoursEnd = c.String(),
                        TotalHours = c.Int(nullable: false),
                        NonPeakOccupancyPercentage = c.Int(nullable: false),
                        PeakOccupancyPercentage = c.Int(nullable: false),
                        IsPeak = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TimeOfDayZones",
                c => new
                    {
                        ZoneId = c.Int(nullable: false, identity: true),
                        ClientModelId = c.Int(nullable: false),
                        ClientId = c.Int(nullable: false),
                        NumberOfSpacesPerZone = c.Int(nullable: false),
                        PercentOfSpaceOccupied = c.Int(nullable: false),
                        NumberOfSpacesRemaining = c.Int(nullable: false),
                        CompliancePercentage = c.Int(nullable: false),
                        ZoneCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ZoneId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ClientId = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Zones",
                c => new
                    {
                        ZoneCode = c.Int(nullable: false, identity: true),
                        ZoneName = c.String(),
                        ClientId = c.Int(nullable: false),
                        ParkingTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ZoneCode);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.Zones");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.TimeOfDayZones");
            DropTable("dbo.TimeOfDayOperatingHours");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Permits");
            DropTable("dbo.PermitDetails");
            DropTable("dbo.ParkingClients");
            DropTable("dbo.OperatingDays");
            DropTable("dbo.LuParkingTypes");
            DropTable("dbo.LuModelTypes");
            DropTable("dbo.LuEquipmentTypes");
            DropTable("dbo.HourlyZones");
            DropTable("dbo.HourlyOperatingHours");
            DropTable("dbo.EscalatingZones");
            DropTable("dbo.EscalatingOperatingHours");
            DropTable("dbo.EquipmentCosts");
            DropTable("dbo.ClientModels");
            DropTable("dbo.ApiErrors");
        }
    }
}
