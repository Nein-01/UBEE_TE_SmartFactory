namespace ATEVersions_Management.Migrations.DevSide
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterPROGRAM_Add2Cols : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PROGRAM", "TestPhase", c => c.String(maxLength: 50));
            AddColumn("dbo.PROGRAM", "ProjectType", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PROGRAM", "ProjectType");
            DropColumn("dbo.PROGRAM", "TestPhase");
        }
    }
}
