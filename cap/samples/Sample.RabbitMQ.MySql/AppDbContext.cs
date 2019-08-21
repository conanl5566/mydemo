using Microsoft.EntityFrameworkCore;

namespace Sample.RabbitMQ.MySql
{
    public class Person
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class AppDbContext : DbContext
    {
      //  public const string ConnectionString = "Server=.;Database=netcore;Uid=sa;Password=P@ssw0rd;";

        public DbSet<Person> Persons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=netcore;User Id=sa;Password=sa;MultipleActiveResultSets=true");
        }
    }
}
