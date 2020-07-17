namespace FinancialModeling.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedGaragesColumnInEqipCostTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EquipmentCosts", "QuantityOfUnits", c => c.Int(nullable: false));
            AddColumn("dbo.EquipmentCosts", "MultiSpaceMeterCost", c => c.Int(nullable: false));
            AddColumn("dbo.EquipmentCosts", "EquipWithBNA", c => c.Int(nullable: false));
            AddColumn("dbo.EquipmentCosts", "EquipWithCreditCard", c => c.Int(nullable: false));
            AddColumn("dbo.EquipmentCosts", "AnnualSoftwareFee", c => c.Int(nullable: false));
            AddColumn("dbo.EquipmentCosts", "IsWarrantyIncluded", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EquipmentCosts", "IsWarrantyIncluded");
            DropColumn("dbo.EquipmentCosts", "AnnualSoftwareFee");
            DropColumn("dbo.EquipmentCosts", "EquipWithCreditCard");
            DropColumn("dbo.EquipmentCosts", "EquipWithBNA");
            DropColumn("dbo.EquipmentCosts", "MultiSpaceMeterCost");
            DropColumn("dbo.EquipmentCosts", "QuantityOfUnits");
        }
    }
}
