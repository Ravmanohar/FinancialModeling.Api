namespace FinancialModeling.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDateFieldChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.AspNetUsers", "ModifiedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "ModifiedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.AspNetUsers", "CreatedDate", c => c.DateTime(nullable: false));
        }
    }
}
