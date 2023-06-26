using AsposeTestTask.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsposeTestTask.DAL.Data
{
    public partial class AsposeContext : DbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<Person> Persons { get; set; }
    }
}
