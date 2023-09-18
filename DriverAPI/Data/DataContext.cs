using DriverAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DriverAPI.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options) 
        {

        }
        public DbSet<Driver> driver { get; set; }
    }
}
