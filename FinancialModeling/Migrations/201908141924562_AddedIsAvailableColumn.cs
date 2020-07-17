namespace FinancialModeling.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedIsAvailableColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientModels", "IsAvailable", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClientModels", "IsAvailable");
        }
    }
}
