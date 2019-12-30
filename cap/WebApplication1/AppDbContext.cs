using Microsoft.EntityFrameworkCore;

namespace WebApplication1
{
    public class Person
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class AppDbContext : DbContext
    {
        //  public const string ConnectionString = "Data Source=.;uid=sa;pwd=P@ssw0rd;database=test;";

        public DbSet<Person> Persons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=netcore;User Id=sa;Password=sa;MultipleActiveResultSets=true");
        }

        //public AppDbContext(DbContextOptions<AppDbContext> options)
        //  : base(options)
        //{
        //}
    }
}