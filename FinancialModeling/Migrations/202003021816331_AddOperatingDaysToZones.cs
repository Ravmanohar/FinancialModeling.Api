namespace FinancialModeling.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOperatingDaysToZones : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Zones", "OperatingDays", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Zones", "OperatingDays");
        }
    }
}
