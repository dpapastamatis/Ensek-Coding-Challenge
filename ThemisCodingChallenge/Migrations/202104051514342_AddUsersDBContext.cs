namespace EnsekCodingChallenge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUsersDBContext : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Employees", name: "User_Id", newName: "LinkedUser_Id");
            RenameIndex(table: "dbo.Employees", name: "IX_User_Id", newName: "IX_LinkedUser_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Employees", name: "IX_LinkedUser_Id", newName: "IX_User_Id");
            RenameColumn(table: "dbo.Employees", name: "LinkedUser_Id", newName: "User_Id");
        }
    }
}
