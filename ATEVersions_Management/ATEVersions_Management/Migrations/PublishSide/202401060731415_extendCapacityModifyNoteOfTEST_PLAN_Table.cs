namespace ATEVersions_Management.Migrations.PublishSide
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class extendCapacityModifyNoteOfTEST_PLAN_Table : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TEST_PLAN", "ModifyNote", c => c.String(nullable: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TEST_PLAN", "ModifyNote", c => c.String(nullable: true, maxLength: 250));
        }
    }
}
