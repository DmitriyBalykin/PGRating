namespace PGRating.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Participants", "RankingDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Pilots", "Rating", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Pilots", "RatingDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pilots", "RatingDate");
            DropColumn("dbo.Pilots", "Rating");
            DropColumn("dbo.Participants", "RankingDate");
        }
    }
}
