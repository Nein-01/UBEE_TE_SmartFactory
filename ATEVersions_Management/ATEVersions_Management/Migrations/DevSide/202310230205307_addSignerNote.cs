namespace ATEVersions_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSignerNote : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ATE_CHECKLIST", "PreparerNote", c => c.String(maxLength: 250));
            AddColumn("dbo.ATE_CHECKLIST", "CheckerNote", c => c.String(maxLength: 250));
            AddColumn("dbo.ATE_CHECKLIST", "ApproverNote", c => c.String(maxLength: 250));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ATE_CHECKLIST", "ApproverNote");
            DropColumn("dbo.ATE_CHECKLIST", "CheckerNote");
            DropColumn("dbo.ATE_CHECKLIST", "PreparerNote");
        }
    }
}
