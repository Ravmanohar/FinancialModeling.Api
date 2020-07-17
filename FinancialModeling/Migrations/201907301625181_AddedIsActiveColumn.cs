namespace FinancialModeling.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedIsActiveColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientModels", "IsActive", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.EquipmentCosts", "IsActive", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.EscalatingOperatingHours", "IsActive", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.EscalatingZones", "IsActive", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.HourlyOperatingHours", "IsActive", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.HourlyZones", "IsActive", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.LuEquipmentTypes", "IsActive", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.LuModelTypes", "IsActive", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.LuParkingTypes", "IsActive", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.OperatingDays", "IsActive", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.ParkingClients", "HavePermits", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.PermitDetails", "IsActive", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.Permits", "IsActive", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.TimeOfDayOperatingHours", "IsActive", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.TimeOfDayZones", "IsActive", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.Zones", "IsActive", c => c.Boolean(nullable: false, defaultValue: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Zones", "IsActive");
            DropColumn("dbo.TimeOfDayZones", "IsActive");
            DropColumn("dbo.TimeOfDayOperatingHours", "IsActive");
            DropColumn("dbo.Permits", "IsActive");
            DropColumn("dbo.PermitDetails", "IsActive");
            DropColumn("dbo.ParkingClients", "HavePermits");
            DropColumn("dbo.OperatingDays", "IsActive");
            DropColumn("dbo.LuParkingTypes", "IsActive");
            DropColumn("dbo.LuModelTypes", "IsActive");
            DropColumn("dbo.LuEquipmentTypes", "IsActive");
            DropColumn("dbo.HourlyZones", "IsActive");
            DropColumn("dbo.HourlyOperatingHours", "IsActive");
            DropColumn("dbo.EscalatingZones", "IsActive");
            DropColumn("dbo.EscalatingOperatingHours", "IsActive");
            DropColumn("dbo.EquipmentCosts", "IsActive");
            DropColumn("dbo.ClientModels", "IsActive");
        }
    }
}
