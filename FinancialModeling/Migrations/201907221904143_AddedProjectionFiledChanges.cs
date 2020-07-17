namespace FinancialModeling.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedProjectionFiledChanges : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Projections");
            DropColumn("dbo.Projections", "ProjectioId");
            AddColumn("dbo.Projections", "ProjectionId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Projections", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Projections", "IsDeleted", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Projections", "ModifiedDate", c => c.DateTime());
            AddPrimaryKey("dbo.Projections", "ProjectionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Projections", "ProjectioId", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.Projections");
            AlterColumn("dbo.Projections", "ModifiedDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Projections", "IsDeleted");
            DropColumn("dbo.Projections", "IsActive");
            DropColumn("dbo.Projections", "ProjectionId");
            AddPrimaryKey("dbo.Projections", "ProjectioId");
        }
    }
}
