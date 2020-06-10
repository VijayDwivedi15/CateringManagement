namespace CateringSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class forUserDBContext : DbMigration
    {
        public override void Up()
        {
           
            CreateTable(
                "dbo.UserContacts",
                c => new
                    {
                        ContactID = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        MobileNo = c.String(),
                        EmailID = c.String(),
                        Subject = c.String(),
                        Message = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ContactID);
            
           
        }
        
        public override void Down()
        {
           
            
            DropTable("dbo.UserContacts");
           
        }
    }
}
