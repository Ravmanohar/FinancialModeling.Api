namespace FinancialModeling.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDecimalColumnChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EquipmentCosts", "MonthlyMeterSoftwareFees", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.EquipmentCosts", "MonthlyCreditCardProcessingFees", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EquipmentCosts", "MonthlyCreditCardProcessingFees", c => c.Int(nullable: false));
            AlterColumn("dbo.EquipmentCosts", "MonthlyMeterSoftwareFees", c => c.Int(nullable: false));
        }
    }
}
