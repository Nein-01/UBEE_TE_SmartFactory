namespace ATEVersions_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addUpdatedByField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ATE_CHECKLIST", "UpdatedBy", c => c.String(maxLength: 50));
            AddColumn("dbo.CHECKLIST_ITEM", "UpdatedBy", c => c.String(maxLength: 50));
            AddColumn("dbo.VERSION", "UpdatedBy", c => c.String(maxLength: 50));
            AddColumn("dbo.PROGRAM", "UpdatedBy", c => c.String(maxLength: 50));
            AddColumn("dbo.USERS", "UpdatedBy", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.USERS", "UpdatedBy");
            DropColumn("dbo.PROGRAM", "UpdatedBy");
            DropColumn("dbo.VERSION", "UpdatedBy");
            DropColumn("dbo.CHECKLIST_ITEM", "UpdatedBy");
            DropColumn("dbo.ATE_CHECKLIST", "UpdatedBy");
        }
    }
}
