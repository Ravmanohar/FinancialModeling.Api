namespace FinancialModeling.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddedEscalatingZoneColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EscalatingZones", "DailyHourlyPercentValuesJson", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.EscalatingZones", "DailyHourlyPercentValuesJson");
        }
    }
}
