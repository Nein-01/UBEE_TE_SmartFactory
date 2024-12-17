namespace ATEVersions_Management.Migrations.DevSide
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addModifiedDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TEST_PLAN", "ModifiedAt", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TEST_PLAN", "ModifiedAt");
        }
    }
}
