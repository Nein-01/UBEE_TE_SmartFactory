namespace ATEVersions_Management.Migrations.PublishSide
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterPROGRAM_TESTPLAN_add2cols : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PROGRAM", "TestPhase", c => c.String(maxLength: 50));
            AddColumn("dbo.PROGRAM", "ProjectType", c => c.String(maxLength: 50));
            AddColumn("dbo.TEST_PLAN", "TestPhase", c => c.String(maxLength: 50));
            AddColumn("dbo.TEST_PLAN", "ProjectType", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TEST_PLAN", "ProjectType");
            DropColumn("dbo.TEST_PLAN", "TestPhase");
            DropColumn("dbo.PROGRAM", "ProjectType");
            DropColumn("dbo.PROGRAM", "TestPhase");
        }
    }
}
