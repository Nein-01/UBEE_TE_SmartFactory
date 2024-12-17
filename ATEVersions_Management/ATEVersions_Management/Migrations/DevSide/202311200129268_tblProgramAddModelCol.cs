namespace ATEVersions_Management.Migrations.DevSide
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tblProgramAddModelCol : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PROGRAM", "ModelName", c => c.String(nullable: false, maxLength: 250));
            /*AlterColumn("dbo.VERSION", "ReleaseTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.VERSION", "ReleaseNote", c => c.String(nullable: false));*/
        }
        
        public override void Down()
        {
            /*AlterColumn("dbo.VERSION", "ReleaseNote", c => c.String());
            AlterColumn("dbo.VERSION", "ReleaseTime", c => c.DateTime());*/
            DropColumn("dbo.PROGRAM", "ModelName");
        }
    }
}
