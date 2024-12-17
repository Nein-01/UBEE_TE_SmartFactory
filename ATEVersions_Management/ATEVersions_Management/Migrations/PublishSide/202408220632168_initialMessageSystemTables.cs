namespace ATEVersions_Management.Migrations.PublishSide
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialMessageSystemTables : DbMigration
    {
        public override void Up()
        {
            /*CreateTable(
                "dbo.MESSAGE_SENDER",
                c => new
                    {
                        MESSAGE_ID = c.Int(nullable: false, identity: true),
                        MESSAGE_PARENT_ID = c.Int(),
                        SENDER_ID = c.Int(nullable: false),
                        MESSAGE_TYPE = c.String(maxLength: 100),
                        MESSAGE_CONTENT = c.String(),
                        DEVICE_NAME_IP = c.String(maxLength: 100),
                        SEND_DATE = c.DateTime(),
                    })
                .PrimaryKey(t => t.MESSAGE_ID)
                .ForeignKey("dbo.USERS", t => t.SENDER_ID, cascadeDelete: true)
                .Index(t => t.SENDER_ID);
            
            CreateTable(
                "dbo.MESSAGE_RECIPIENT",
                c => new
                    {
                        RECEIVE_ID = c.Int(nullable: false, identity: true),
                        MESSAGE_ID = c.Int(nullable: false),
                        RECIPIENT_ID = c.Int(),
                        RECIPIENT_GROUP_ID = c.Int(),
                        RECEIVE_DATE = c.DateTime(),
                    })
                .PrimaryKey(t => t.RECEIVE_ID)
                .ForeignKey("dbo.MESSAGE_SENDER", t => t.MESSAGE_ID, cascadeDelete: true)
                .Index(t => t.MESSAGE_ID);
            
            CreateTable(
                "dbo.MESSAGE_USER_GROUP",
                c => new
                    {
                        USER_GROUP_ID = c.Int(nullable: false, identity: true),
                        USER_ID = c.Int(nullable: false),
                        GROUP_ID = c.Int(nullable: false),
                        ROLE_IN_GROUP = c.String(maxLength: 30),
                        CREATE_DATE = c.DateTime(),
                        IS_ACTIVE = c.Boolean(),
                    })
                .PrimaryKey(t => t.USER_GROUP_ID)
                .ForeignKey("dbo.MESSAGE_GROUP", t => t.GROUP_ID, cascadeDelete: true)
                .ForeignKey("dbo.USERS", t => t.USER_ID, cascadeDelete: true)
                .Index(t => t.USER_ID)
                .Index(t => t.GROUP_ID);
            
            CreateTable(
                "dbo.MESSAGE_GROUP",
                c => new
                    {
                        GROUP_ID = c.Int(nullable: false, identity: true),
                        GROUP_NAME = c.String(nullable: false, maxLength: 200),
                        CREATE_BY = c.Int(),
                        CREATE_DATE = c.DateTime(),
                        IS_ACTIVE = c.Boolean(),
                    })
                .PrimaryKey(t => t.GROUP_ID);
            */
        }
        
        public override void Down()
        {
            /*DropForeignKey("dbo.MESSAGE_USER_GROUP", "USER_ID", "dbo.USERS");
            DropForeignKey("dbo.MESSAGE_USER_GROUP", "GROUP_ID", "dbo.MESSAGE_GROUP");
            DropForeignKey("dbo.MESSAGE_SENDER", "SENDER_ID", "dbo.USERS");
            DropForeignKey("dbo.MESSAGE_RECIPIENT", "MESSAGE_ID", "dbo.MESSAGE_SENDER");
            DropIndex("dbo.MESSAGE_USER_GROUP", new[] { "GROUP_ID" });
            DropIndex("dbo.MESSAGE_USER_GROUP", new[] { "USER_ID" });
            DropIndex("dbo.MESSAGE_RECIPIENT", new[] { "MESSAGE_ID" });
            DropIndex("dbo.MESSAGE_SENDER", new[] { "SENDER_ID" });
            DropTable("dbo.MESSAGE_GROUP");
            DropTable("dbo.MESSAGE_USER_GROUP");
            DropTable("dbo.MESSAGE_RECIPIENT");
            DropTable("dbo.MESSAGE_SENDER");*/
        }
    }
}
