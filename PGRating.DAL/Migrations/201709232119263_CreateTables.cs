namespace PGRating.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Competitions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        QualityCoefficient = c.Double(nullable: false),
                        TimeCoefficient = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Participants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rating = c.Double(nullable: false),
                        Pilot_Id = c.Int(),
                        Competition_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pilots", t => t.Pilot_Id)
                .ForeignKey("dbo.Competitions", t => t.Competition_Id)
                .Index(t => t.Pilot_Id)
                .Index(t => t.Competition_Id);
            
            CreateTable(
                "dbo.Pilots",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Nation_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Nations", t => t.Nation_Id)
                .Index(t => t.Nation_Id);
            
            CreateTable(
                "dbo.Nations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NationTeamParticipants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rank = c.Int(nullable: false),
                        Name = c.String(),
                        Rating = c.Double(nullable: false),
                        CR1 = c.Double(nullable: false),
                        CQ1 = c.Double(nullable: false),
                        CR2 = c.Double(nullable: false),
                        CQ2 = c.Double(nullable: false),
                        CR3 = c.Double(nullable: false),
                        CQ3 = c.Double(nullable: false),
                        CR4 = c.Double(nullable: false),
                        CQ4 = c.Double(nullable: false),
                        EquivalentRating = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Participants", "Competition_Id", "dbo.Competitions");
            DropForeignKey("dbo.Participants", "Pilot_Id", "dbo.Pilots");
            DropForeignKey("dbo.Pilots", "Nation_Id", "dbo.Nations");
            DropIndex("dbo.Pilots", new[] { "Nation_Id" });
            DropIndex("dbo.Participants", new[] { "Competition_Id" });
            DropIndex("dbo.Participants", new[] { "Pilot_Id" });
            DropTable("dbo.NationTeamParticipants");
            DropTable("dbo.Nations");
            DropTable("dbo.Pilots");
            DropTable("dbo.Participants");
            DropTable("dbo.Competitions");
        }
    }
}
