namespace cpuListApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addChecker : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CPUs", "ReleaseDate", c => c.Int(nullable: false));
            AddColumn("dbo.CPUs", "Cores", c => c.Int(nullable: false));
            AddColumn("dbo.CPUs", "Threads", c => c.Int(nullable: false));
            AddColumn("dbo.CPUs", "FreqDefault", c => c.Int(nullable: false));
            AddColumn("dbo.CPUs", "FreqTurbo", c => c.Int(nullable: false));
            AddColumn("dbo.CPUs", "Techproccess", c => c.Int(nullable: false));
            AddColumn("dbo.CPUs", "L1cache", c => c.Int(nullable: false));
            AddColumn("dbo.CPUs", "L2cache", c => c.Int(nullable: false));
            AddColumn("dbo.CPUs", "L3cache", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CPUs", "L3cache");
            DropColumn("dbo.CPUs", "L2cache");
            DropColumn("dbo.CPUs", "L1cache");
            DropColumn("dbo.CPUs", "Techproccess");
            DropColumn("dbo.CPUs", "FreqTurbo");
            DropColumn("dbo.CPUs", "FreqDefault");
            DropColumn("dbo.CPUs", "Threads");
            DropColumn("dbo.CPUs", "Cores");
            DropColumn("dbo.CPUs", "ReleaseDate");
        }
    }
}
