using AsposeTestTask.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsposeTestTask.DAL.Data
{
    public partial class AsposeContext : DbContext
    {
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Person> Persons { get; set; }
    }
}
