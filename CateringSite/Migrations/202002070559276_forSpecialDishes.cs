namespace CateringSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class forSpecialDishes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SpecialDishes",
                c => new
                    {
                        DishID = c.Long(nullable: false, identity: true),
                        DishName = c.String(),
                        DishImage = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DishID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SpecialDishes");
        }
    }
}
