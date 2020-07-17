namespace FinancialModeling.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedProjectionTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Projections",
                c => new
                    {
                        ProjectioId = c.Int(nullable: false, identity: true),
                        ProjectionName = c.String(),
                        ClientId = c.Int(nullable: false),
                        UserId = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedById = c.String(),
                        ModifiedById = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        FinancialDashboardJson = c.String(),
                    })
                .PrimaryKey(t => t.ProjectioId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Projections");
        }
    }
}
