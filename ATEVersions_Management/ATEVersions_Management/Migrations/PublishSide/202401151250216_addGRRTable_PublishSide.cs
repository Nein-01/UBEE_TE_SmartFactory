namespace ATEVersions_Management.Migrations.PublishSide
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addGRRTable_PublishSide : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GRR_TABLE",
                c => new
                    {
                        GRR_ID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        Dept = c.String(nullable: true, maxLength: 50),
                        GageModel = c.String(nullable: true, maxLength: 50),
                        GageName = c.String(nullable: true, maxLength: 50),
                        GageNo = c.String(nullable: true, maxLength: 50),
                        PartName = c.String(nullable: true, maxLength: 250),
                        Specification = c.String(nullable: true, maxLength: 50),
                        Characteristic = c.String(nullable: true, maxLength: 250),
                        JSON_OperTestResult = c.String(),
                        PreparedBy = c.String(nullable: true, maxLength: 50),
                        PreparedAt = c.DateTime(),
                        PreparedNote = c.String(nullable: true, maxLength: 500),
                        ApprovedBy = c.String(nullable: true, maxLength: 50),
                        ApprovedAt = c.DateTime(),
                        ApproverNote = c.String(nullable: true, maxLength: 250),
                        Status = c.Int(),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                        UpdatedBy = c.String(nullable: true, maxLength: 50),
                    })
                .PrimaryKey(t => t.GRR_ID)
                .ForeignKey("dbo.USERS", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            AlterColumn("dbo.TEST_PLAN", "Author", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GRR_TABLE", "UserID", "dbo.USERS");
            DropIndex("dbo.GRR_TABLE", new[] { "UserID" });
            AlterColumn("dbo.TEST_PLAN", "Author", c => c.String(nullable: false, maxLength: 50));
            DropTable("dbo.GRR_TABLE");
        }
    }
}
