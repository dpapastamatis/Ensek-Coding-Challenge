namespace EnsekCodingChallenge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameEmployeeFields : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Employees", name: "LinkedUser_Id", newName: "Linked_User_Id");
            RenameIndex(table: "dbo.Employees", name: "IX_LinkedUser_Id", newName: "IX_Linked_User_Id");
            AddColumn("dbo.Employees", "Employee_Name", c => c.String(nullable: false));
            AddColumn("dbo.Employees", "Employee_Salary", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "Employee_Age", c => c.Int(nullable: false));
            DropColumn("dbo.Employees", "Name");
            DropColumn("dbo.Employees", "Salary");
            DropColumn("dbo.Employees", "Age");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employees", "Age", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "Salary", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "Name", c => c.String(nullable: false));
            DropColumn("dbo.Employees", "Employee_Age");
            DropColumn("dbo.Employees", "Employee_Salary");
            DropColumn("dbo.Employees", "Employee_Name");
            RenameIndex(table: "dbo.Employees", name: "IX_Linked_User_Id", newName: "IX_LinkedUser_Id");
            RenameColumn(table: "dbo.Employees", name: "Linked_User_Id", newName: "LinkedUser_Id");
        }
    }
}
