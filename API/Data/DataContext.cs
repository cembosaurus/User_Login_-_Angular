using Users.Models;
using Microsoft.EntityFrameworkCore;

namespace Users.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> option) :
            base(option) {}

        public DbSet<Value> Values { get; set; }

        public DbSet<User> Users { get; set; }



    }

    
}