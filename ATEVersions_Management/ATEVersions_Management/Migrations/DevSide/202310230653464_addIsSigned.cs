namespace ATEVersions_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIsSigned : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ATE_CHECKLIST", "IsChecked", c => c.Int());
            AddColumn("dbo.ATE_CHECKLIST", "IsApproved", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ATE_CHECKLIST", "IsApproved");
            DropColumn("dbo.ATE_CHECKLIST", "IsChecked");
        }
    }
}
