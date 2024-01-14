namespace cpuListApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Brands",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CPUs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Segment = c.String(),
                        Multiplier = c.Boolean(),
                        Arch = c.String(),
                        TDP = c.Single(nullable: false),
                        TempLimit = c.Single(nullable: false),
                        APU = c.Boolean(),
                        Name = c.String(),
                        BenchPoints = c.Single(nullable: false),
                        Rank = c.Int(nullable: false),
                        SocketId = c.Int(nullable: false),
                        BrandId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Brands", t => t.BrandId, cascadeDelete: true)
                .ForeignKey("dbo.Sockets", t => t.SocketId, cascadeDelete: true)
                .Index(t => t.SocketId)
                .Index(t => t.BrandId);
            
            CreateTable(
                "dbo.Sockets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CPUs", "SocketId", "dbo.Sockets");
            DropForeignKey("dbo.CPUs", "BrandId", "dbo.Brands");
            DropIndex("dbo.CPUs", new[] { "BrandId" });
            DropIndex("dbo.CPUs", new[] { "SocketId" });
            DropTable("dbo.Sockets");
            DropTable("dbo.CPUs");
            DropTable("dbo.Brands");
        }
    }
}
