using Model;
using System.Data.Entity;

namespace DAL
{
    public class HomeWorkDB : DbContext
    {
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Department> Department { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().ToTable("Employee");
            modelBuilder.Entity<Department>().ToTable("Department");
            base.OnModelCreating(modelBuilder);
        }

        static HomeWorkDB()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<HomeWorkDB>());
        }
    }
}
