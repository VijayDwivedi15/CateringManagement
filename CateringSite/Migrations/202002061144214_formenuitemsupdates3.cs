namespace CateringSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class formenuitemsupdates3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.MenuItemDetails", "ItemPrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MenuItemDetails", "ItemPrice", c => c.Single());
        }
    }
}
