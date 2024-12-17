namespace ATEVersions_Management.Migrations.DevSide
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createGRR_TABLE : DbMigration
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
                        PartName = c.String(nullable: true, maxLength: 100),
                        Specification = c.String(nullable: true, maxLength: 50),
                        Characteristic = c.String(nullable: true, maxLength: 250),
                        JSON_OperTestResult = c.String(nullable: true),
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GRR_TABLE", "UserID", "dbo.USERS");
            DropIndex("dbo.GRR_TABLE", new[] { "UserID" });
            DropTable("dbo.GRR_TABLE");
        }
    }
}
