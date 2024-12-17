namespace ATEVersions_Management.Migrations.PublishSide
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterUSERS_AddDeptCol : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.USERS", "Department", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.USERS", "Department");
        }
    }
}
