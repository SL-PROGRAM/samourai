namespace samsam.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class samsam2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Samourais", "Potentiel", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Samourais", "Potentiel");
        }
    }
}
