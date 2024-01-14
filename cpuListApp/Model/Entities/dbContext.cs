using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace cpuListApp.Model.Entities
{
    public partial class cpuListContext : DbContext
    {
        public cpuListContext()
            : base("name=dbContext")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CPU>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Socket>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Brand>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //throw new UnintentionalCodeFirstException();
        }

        public DbSet<CPU> CPUs { get; set; }
        public DbSet<Socket> Sockets { get; set; }
        public DbSet<Brand> Brands { get; set; }
    }
}
