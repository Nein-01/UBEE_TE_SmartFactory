namespace ATEVersions_Management.Migrations.DevSide
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterTESTPLAN_Add2Cols : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TEST_PLAN", "TestPhase", c => c.String(maxLength: 50));
            AddColumn("dbo.TEST_PLAN", "ProjectType", c => c.String(maxLength: 50));
            AlterColumn("dbo.PROGRAM", "TestPhase", c => c.String(maxLength: 50));
            AlterColumn("dbo.PROGRAM", "ProjectType", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PROGRAM", "ProjectType", c => c.String(maxLength: 250));
            AlterColumn("dbo.PROGRAM", "TestPhase", c => c.String(maxLength: 250));
            DropColumn("dbo.TEST_PLAN", "ProjectType");
            DropColumn("dbo.TEST_PLAN", "TestPhase");
        }
    }
}
