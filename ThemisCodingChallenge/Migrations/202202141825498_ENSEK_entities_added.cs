namespace EnsekCodingChallenge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ENSEK_entities_added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        AccountID = c.Int(nullable: false, identity: true),
                        First_name = c.String(nullable: false),
                        Last_name = c.String(nullable: false),
                        Linked_User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.AccountID)
                .ForeignKey("dbo.AspNetUsers", t => t.Linked_User_Id)
                .Index(t => t.Linked_User_Id);
            
            CreateTable(
                "dbo.MeterReadings",
                c => new
                    {
                        MeterReadingID = c.Int(nullable: false, identity: true),
                        AccountID = c.Int(nullable: false),
                        MeterReadingDateTime = c.DateTime(nullable: false),
                        MeterReadValue = c.Long(nullable: false),
                        Linked_User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.MeterReadingID)
                .ForeignKey("dbo.AspNetUsers", t => t.Linked_User_Id)
                .Index(t => t.Linked_User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MeterReadings", "Linked_User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Accounts", "Linked_User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.MeterReadings", new[] { "Linked_User_Id" });
            DropIndex("dbo.Accounts", new[] { "Linked_User_Id" });
            DropTable("dbo.MeterReadings");
            DropTable("dbo.Accounts");
        }
    }
}
