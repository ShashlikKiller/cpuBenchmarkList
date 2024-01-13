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
            modelBuilder.Entity<CPU>().ToTable("CPUs");
            //throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<CPU> CPUs { get; set; }
    }
}
