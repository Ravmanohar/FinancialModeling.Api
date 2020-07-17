namespace FinancialModeling.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDataTypeChanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EscalatingZones", "NonPeakHourlyRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.EscalatingZones", "NonPeakEscalatingRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.EscalatingZones", "NonPeakHourEscalatingRateBegins", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.EscalatingZones", "NonPeakDailyMaxOrAllDayRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.EscalatingZones", "NonPeakEveningFlatRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.EscalatingZones", "PeakHourlyRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.EscalatingZones", "PeakEscalatingRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.EscalatingZones", "PeakHourEscalatingRateBegins", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.EscalatingZones", "PeakDailyMaxOrAllDayRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.EscalatingZones", "PeakEveningFlatRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.HourlyZones", "NonPeakSeasonHourlyRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.HourlyZones", "PeakSeasonHourlyRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.TimeOfDayOperatingHours", "PeakSeasonHourlyRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.TimeOfDayOperatingHours", "NonPeakSeasonHourlyRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TimeOfDayOperatingHours", "NonPeakSeasonHourlyRate", c => c.Int(nullable: false));
            AlterColumn("dbo.TimeOfDayOperatingHours", "PeakSeasonHourlyRate", c => c.Int(nullable: false));
            AlterColumn("dbo.HourlyZones", "PeakSeasonHourlyRate", c => c.Int(nullable: false));
            AlterColumn("dbo.HourlyZones", "NonPeakSeasonHourlyRate", c => c.Int(nullable: false));
            AlterColumn("dbo.EscalatingZones", "PeakEveningFlatRate", c => c.Int(nullable: false));
            AlterColumn("dbo.EscalatingZones", "PeakDailyMaxOrAllDayRate", c => c.Int(nullable: false));
            AlterColumn("dbo.EscalatingZones", "PeakHourEscalatingRateBegins", c => c.Int(nullable: false));
            AlterColumn("dbo.EscalatingZones", "PeakEscalatingRate", c => c.Int(nullable: false));
            AlterColumn("dbo.EscalatingZones", "PeakHourlyRate", c => c.Int(nullable: false));
            AlterColumn("dbo.EscalatingZones", "NonPeakEveningFlatRate", c => c.Int(nullable: false));
            AlterColumn("dbo.EscalatingZones", "NonPeakDailyMaxOrAllDayRate", c => c.Int(nullable: false));
            AlterColumn("dbo.EscalatingZones", "NonPeakHourEscalatingRateBegins", c => c.Int(nullable: false));
            AlterColumn("dbo.EscalatingZones", "NonPeakEscalatingRate", c => c.Int(nullable: false));
            AlterColumn("dbo.EscalatingZones", "NonPeakHourlyRate", c => c.Int(nullable: false));
        }
    }
}
